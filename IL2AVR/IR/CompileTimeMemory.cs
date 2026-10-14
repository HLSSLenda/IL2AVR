using System;
using System.Buffers;
using System.Collections.Generic;
namespace IL2AVR.IR;

public sealed class CompileTimeMemory : IDisposable
{
    private readonly ArrayPool<byte> _pool;
    private byte[]? _array;
    public readonly CompileTimeMemoryLocalOrder MemoryOrder;
    private readonly LinkedList<CompileTimeLocal> _objects;
    public IReadOnlyCollection<CompileTimeLocal> Objects => _objects;
    public ushort StartPointer;
    public CompileTimeMemory(ushort size, CompileTimeMemoryLocalOrder order = CompileTimeMemoryLocalOrder.Down, ArrayPool<byte>? pool = null)
    {
        _pool = pool ?? ArrayPool<byte>.Shared;
        _array = _pool.Rent(size);
        _objects = new();
        _objects.AddFirst(new CompileTimeLocal(new Void(size), RealOffset(0)));
        MemoryOrder = order;
        StartPointer = (ushort)RealOffset(0);
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
        ushort offset = 0;
        while (true)
        {
            local = localNode.Value;
            if (offset >= StartPointer && local.Type is Void && value.Type.Size <= local.Type.Size)
            {
                ushort size = (ushort)(local.Type.Size - value.Type.Size);
                if (size > 0)
                    // we split the used and unused memory
                    _objects.AddAfter(localNode, new CompileTimeLocal(new Void(size), local.Offset + size));
                local.Value = value;
                return local;
            }
            if (localNode.Next is null)
                break;
        }
        offset = Math.Max(offset, StartPointer);
        if (VirtualOffset(offset) + value.Type.Size > _array!.Length)
            throw new InvalidOperationException("Out of memory!");
        CompileTimeLocal newLocal = new(value, offset + value.Type.Size);
        _objects.AddLast(newLocal);
        if (!newLocal.TrySerialize(new(_array, RealOffset(newLocal.Offset), value.Type.Size)))
            throw new WtfException();
        return newLocal;
    }

    public bool Remove(ICompileTimeValue value)
    {
        ThrowIfDisposed();
        LinkedListNode<CompileTimeLocal>? node = _objects.First;
        while (node is not null)
        {
            CompileTimeLocal local = node.Value;
            if (object.ReferenceEquals(value, local.Value))
            {
                int newOffset = local.Offset;
                ushort totalFreed = local.Type.Size;
                if (node.Previous?.Value.Value is Void)
                {
                    newOffset = node.Previous.Value.Offset;
                    totalFreed += node.Previous.Value.Type.Size;
                    _objects.Remove(node.Previous);
                }
                if (node.Next?.Value.Value is Void)
                {
                    totalFreed += node.Next.Value.Type.Size;
                    _objects.Remove(node.Next);
                }
                node.ValueRef = new(new Void(totalFreed), newOffset);
                return true;
            }
            node = node.Next;
        }
        return false;
    }

    public bool Pop()
    {
        ThrowIfDisposed();

    }
}
