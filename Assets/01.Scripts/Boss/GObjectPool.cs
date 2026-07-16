using System.Collections.Generic;
using UnityEngine;

public class GObjectPool<T> : MonoBehaviour where T : Component, IPoolable
{
    [SerializeField] private T objects;
    [SerializeField] private int poolSize = 5;

    private Queue<T> pool = new Queue<T>();

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            T obj = Instantiate(objects, transform);
            obj.Init(ReturnPool);

            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public T GetObject(Vector3 position)
    {
        T obj;

        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            obj = Instantiate(objects, transform);
            obj.Init(ReturnPool);
        }

        obj.transform.position = position;
        obj.gameObject.SetActive(true);
        obj.OnSpawn();

        return obj;
    }

    public void ReturnPool(Component comp)
    {
        T obj = comp as T;
        if (obj != null)
        {
            obj.OnDeSpawn();
            obj.gameObject.SetActive(false);

            pool.Enqueue(obj);
        }
    }
}

