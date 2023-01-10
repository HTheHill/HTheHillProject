using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;
    Queue<Tile> objectPool = new Queue<Tile>();
    public Queue<Tile> GetObjectPool { get { return objectPool; } }

    int maxPoolSize;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    public void SetPoolMaxSize(int maxSize)
    {
        maxPoolSize = maxSize;
    }
    public void FillObjectPool(Tile obj)
    {
        obj.gameObject.SetActive(false);
        objectPool.Enqueue(obj);
    }
    public Tile SpawnFromPool()
    {
        Tile objecToSpawn = objectPool.Dequeue();
        objecToSpawn.gameObject.SetActive(true);
        return objecToSpawn;
    }
    public void GiveBackToPool(Tile tile)
    {
        tile.gameObject.SetActive(false);
        objectPool.Enqueue(tile);
    }

}
