using System.Collections.Generic;

public interface IGenericRepository<T>
{
    void Add(T item);
    bool Contains(T item);
    List<T> GetAll();
    int Count { get; }
    T this[int index] { get; }
}