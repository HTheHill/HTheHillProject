using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManagerVer2 : MonoBehaviour
{
    public static PoolManagerVer2 instance;
    Queue<TestTile> objectPool = new Queue<TestTile>();
    public Queue<TestTile> GetObjectPool { get { return objectPool; } }

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
    public void FillObjectPool(TestTile obj)
    {
        obj.gameObject.SetActive(false);
        objectPool.Enqueue(obj);
    }
    public TestTile SpawnFromPool()
    {
        TestTile objecToSpawn = objectPool.Dequeue();
        objecToSpawn.gameObject.SetActive(true);
        return objecToSpawn;
    }
    public void GiveBackToPool(TestTile tile)
    {
        tile.gameObject.SetActive(false);
        objectPool.Enqueue(tile);
    }

}
