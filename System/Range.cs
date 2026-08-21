namespace System;

public readonly struct Range(Index start, Index end)
{
    public Index Start { get; } = start;
    public Index End { get; } = end;
}