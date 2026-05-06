using System;
using System.Collections.Generic;

public class GenericRepository<T> : IGenericRepository<T>
{
    private readonly List<T> _items = new List<T>();

    public void Add(T item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        _items.Add(item);
    }

    public bool Contains(T item)
    {
        if (item == null) return false;
        return _items.Contains(item);
    }

    public List<T> GetAll()
    {
        return new List<T>(_items);
    }

    public int Count => _items.Count;

    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= _items.Count)
                throw new IndexOutOfRangeException();

            return _items[index];
        }
    }
}