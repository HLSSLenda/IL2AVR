namespace System;

public interface IFormatProvider
{
    object GetFormat(Type type);
}