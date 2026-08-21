namespace IL2AVR;

/// 
public interface IObjectPool<T> where T : new()
{
    T Get();
    bool Return(T item);
}
