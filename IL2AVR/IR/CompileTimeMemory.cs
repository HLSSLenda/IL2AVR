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
    private readonly LinkedList<CompileTimeLocal> _objects;
    public IReadOnlyCollection<CompileTimeLocal> Objects => _objects;
    public int AdderOffset;
    public CompileTimeMemory(ushort size, CompileTimeMemoryLocalOrder order = CompileTimeMemoryLocalOrder.Down, ArrayPool<byte>? pool = null)
    {
        _pool = pool ?? ArrayPool<byte>.Shared;
        _array = _pool.Rent(size);
        _objects = new();
        _objects.AddFirst(new CompileTimeLocal(new Void(size), RealOffset(0)));
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
        LinkedListNode<CompileTimeLocal> localNode = _objects.First!;
        CompileTimeLocal local;
        while (true)
        {
            local = localNode.Value;
            if (local.Type is Void && value.Type.Size <= local.Type.Size)
            {
                ushort size = (ushort)(local.Type.Size - value.Type.Size);
                if (size > 0)
                    // we split the used and unused memory
                    _objects.AddAfter(localNode, new CompileTimeLocal(new Void(size)));
                local.Value = value;
                return local;
            }
            if (localNode.Next is null)
                break;
        }
        if (VirtualOffset(offset) + value.Type.Size > _array!.Length)
            throw new InvalidOperationException("Out of memory!");
        CompileTimeLocal newLocal = new(value, );
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
