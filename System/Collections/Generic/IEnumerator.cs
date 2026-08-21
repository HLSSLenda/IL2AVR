namespace System.Collections.Generic;

public interface IEnumerator<out T> : IEnumerator where T : allows ref struct
{
    new T Current { get; }
}