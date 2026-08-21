namespace System;

public readonly struct Boolean
{
    public const string FalseString = "False", TrueString = "True";
    public override string ToString() => this ? TrueString : FalseString;
}