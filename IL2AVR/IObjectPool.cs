namespace IL2AVR;

/// <summary>Represents a pool of objects.</summary>
public interface IObjectPool<T> where T : new()
{
    /// <summary>Gets an instance of <t from the pool.</summary>
    /// <returns>A fresh <typeparamref name="T" /> from the pool.</summary>
    T Get();
    /// <summary>Returns or gives an item to the pool.</summary>
    /// <param name="item">The item to return/give.</param>
    /// <returns>A bool indicating if it's returned successfully.</returns>
    bool Return(T item);
}
