using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Простой Object Pool для переиспользования объектов.
/// Критично для WebGL: предотвращает сборку мусора и лаги.
/// </summary>
public class ObjectPool<T> where T : MonoBehaviour
{
    private readonly Stack<T> _pool = new Stack<T>();
    private readonly Transform _parent;
    private readonly T _prefab;

    public ObjectPool(T prefab, int initialSize, Transform parent)
    {
        _prefab = prefab;
        _parent = parent;

        // Предварительно создаём пул
        for (int i = 0; i < initialSize; i++)
        {
            T item = CreateNew();
            item.gameObject.SetActive(false);
            _pool.Push(item);
        }
    }

    public T Get()
    {
        T item;

        if (_pool.Count > 0)
        {
            item = _pool.Pop();
        }
        else
        {
            item = CreateNew();
        }

        item.gameObject.SetActive(true);
        return item;
    }

    public void Return(T item)
    {
        item.gameObject.SetActive(false);
        _pool.Push(item);
    }

    public void ReturnAll(IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            Return(item);
        }
    }

    private T CreateNew()
    {
        T item = Object.Instantiate(_prefab, _parent);
        item.gameObject.SetActive(false);
        return item;
    }
}
