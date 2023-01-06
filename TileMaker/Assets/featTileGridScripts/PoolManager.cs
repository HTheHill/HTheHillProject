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
    public void SpawnFromPool(float xPos, float yPos, Quaternion rotation)
    {
        Tile objecToSpawn = objectPool.Dequeue();
        objecToSpawn.gameObject.SetActive(true);
        objecToSpawn.transform.position = new Vector3(xPos, yPos, objecToSpawn.transform.position.z);
        objecToSpawn.transform.rotation = rotation;

    }
    public void GiveBackToPool(Tile tile)
    {
        tile.gameObject.SetActive(false);
        objectPool.Enqueue(tile);
    }

}
