using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
namespace IL2AVR.IR;

public sealed class CompileTimeMemory : IDisposable
{
    private readonly ArrayPool<byte> _pool;
    private byte[]? _array;
    public readonly CompileTimeMemoryLocalOrder MemoryOrder;
    private readonly List<CompileTimeLocal> _objects;
    public IReadOnlyList<CompileTimeLocal> Objects => _objects;
    public int AdderOffset;
    public CompileTimeMemory(int size, CompileTimeMemoryLocalOrder order = CompileTimeMemoryLocalOrder.Down, ArrayPool<byte>? pool = null)
    {
        _pool = pool ?? ArrayPool<byte>.Shared;
        _array = _pool.Rent(size);
        _objects = [new(null, RealOffset(0))];
        MemoryOrder = order;
    }

    public void Dispose()
    {
        if (_array is not null)
        {
            _pool.Return(_array);
            _array = null;
        }
    }

    private void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(_array is null, this);

    public int Length
    {
        get
        {
            ThrowIfDisposed();
            return _array!.Length;
        }
    }
    public ReadOnlySpan<byte> Memory
    {
        get
        {
            ThrowIfDisposed();
            return new(_array!);
        }
    }

    private int RealOffset(int voffset) => MemoryOrder == CompileTimeMemoryLocalOrder.Down ? voffset : -voffset + _array!.Length - 1;
    private int VirtualOffset(int roffset) => MemoryOrder == CompileTimeMemoryLocalOrder.Down ? roffset : _array!.Length - 1 - roffset;

    public CompileTimeLocal Add(ICompileTimeValue value)
    {
        ThrowIfDisposed();
        foreach (CompileTimeLocal local in _objects)
        {
            if (!local.IsUsed && value.Type.Size <= local.Type.Size)
            {
                local.IsUsed = true;
                local.Value = value;
                return local;
            }
        }
        CompileTimeLocal lastLocal = _objects[^ - 1];
        CompileTimeLocal newLocal;
        if (VirtualOffset(lastLocal.Offset) + value.Type.Size > _array!.Length)
            throw new InvalidOperationException("Out of memory!");
        newLocal = new(value, RealOffset(VirtualOffset(lastLocal.Offset) + value.Type.Size));
        _objects.Add(newLocal);
        if (!newLocal.TrySerialize(new(_array, RealOffset(newLocal.Offset), value.Type.Size)))
            throw new WtfException();
        return newLocal;
    }

    public bool Remove(ICompileTimeValue value)
    {
        ThrowIfDisposed();
        int minNonUsedRange = -1;
        bool found = false;
        int count = _objects.Count;
        for (int i = 0; i < count; i++)
        {
            CompileTimeLocal local = _objects[i];
            if (found = object.ReferenceEquals(local.Value, value))
            {
                Debug.Assert(local.IsUsed);
                local.IsUsed = false;
            }
            if (minNonUsedRange == -1 && !local.IsUsed)
                minNonUsedRange = i;
            else if (minNonUsedRange != 1 && local.IsUsed)
            {
                _objects.RemoveRange(minNonUsedRange, i - minNonUsedRange);
                count -= i - minNonUsedRange;
            }
        }
        return found;
    }
}
