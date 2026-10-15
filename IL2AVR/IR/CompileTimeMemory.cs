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

    public ReadOnlySpan<byte> FullMemory
    {
        get
        {
            ThrowIfDisposed();
            return new(_array!);
        }
    }

    public ReadOnlySpan<byte> Memory
    {
        get
        {
            ThrowIfDisposed();
            return MemoryOrder == CompileTimeMemoryLocalOrder.Down ?
                new(_array!, 0, StartPointer) :
                new(_array!, StartPointer, _array!.Length - StartPointer);
        }
    }

    private int _bytesOccupied = -1;
    public int BytesOccupied
    {
        get
        {
            ThrowIfDisposed();
            if (_bytesOccupied != -1)
                return _bytesOccupied;
            LinkedListNode<CompileTimeLocal>? localNode = _objects.First!;
            ushort offset = 0;
            ushort sp = (ushort)VirtualStartPointer();
            _bytesOccupied = 0;
            while (localNode is not null)
            {
                CompileTimeLocal local = localNode.Value;
                if (offset < sp && local.Type is not Void)
                    _bytesOccupied += local.Type.Size;
                offset += (ushort)local.Offset;
                localNode = localNode.Next;
            }
            return _bytesOccupied;
        }
    }

    public int LastOffset
    {
        get
        {
            if (_lastAddRearOffset != -1)
                return _lastAddRearOffset;
            LinkedListNode<CompileTimeLocal>? localNode = _objects.Last!;
            ushort offset = 0;
            ushort sp = (ushort)VirtualStartPointer();
            while (localNode.Value.Offset >= 
        }
    }

    private int RealOffset(int voffset) => MemoryOrder == CompileTimeMemoryLocalOrder.Down ? voffset : -voffset + _array!.Length - 1;
    private int VirtualStartPointer() => MemoryOrder == CompileTimeMemoryLocalOrder.Down ? VirtualOffset(StartPointer) : StartPointer;
    private int VirtualOffset(int roffset) => MemoryOrder == CompileTimeMemoryLocalOrder.Down ? roffset : _array!.Length - 1 - roffset;

    private int _lastAddRearOffset = -1;
    private bool _canRearOffsetBeUsed = false;
    public CompileTimeLocal Add(ICompileTimeValue value)
    {
        ThrowIfDisposed();
        _bytesOccupied = -1;
        ushort sp = (ushort)VirtualStartPointer();
        ushort offset;
        if (_lastAddRearOffset == -1)
        {
            LinkedListNode<CompileTimeLocal>? localNode = _objects.First!;
            offset = 0;
            while (localNode is not null)
            {
                CompileTimeLocal local = localNode.Value;
                if (offset >= sp && local.Type is Void && value.Type.Size <= local.Type.Size)
                {
                    ushort size = (ushort)(local.Type.Size - value.Type.Size);
                    if (size > 0)
                        // we split the used and unused memory
                        _objects.AddAfter(localNode, new CompileTimeLocal(new Void(size), local.Offset + size));
                    local.Value = value;
                    return local;
                }
                offset += (ushort)local.Offset;
                localNode = localNode.Next;
            }
        }
        else
            offset = (ushort)_lastAddRearOffset;
        offset = Math.Max(offset, sp);
        if (VirtualOffset(offset) + value.Type.Size > _array!.Length)
            throw new InvalidOperationException("Out of memory!");
        _canRearOffsetBeUsed = true;
        CompileTimeLocal newLocal = new(value, _lastAddRearOffset = offset + value.Type.Size);
        _objects.AddLast(newLocal);
        if (!newLocal.TrySerialize(new(_array, RealOffset(newLocal.Offset), value.Type.Size)))
            throw new WtfException();
        return newLocal;
    }

    public bool Remove(ICompileTimeValue value)
    {
        ThrowIfDisposed();
        _bytesOccupied = -1;
        _canRearOffsetBeUsed = false;
        LinkedListNode<CompileTimeLocal>? node = _objects.First;
        while (node is not null)
        {
            CompileTimeLocal local = node.Value;
            if (object.ReferenceEquals(value, node.ValueRef.Value))
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
}
