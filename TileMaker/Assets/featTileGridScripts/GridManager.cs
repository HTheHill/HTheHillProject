using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int width, height;
    [SerializeField] Tile tilePrefab;
    [SerializeField] GameObject gridSys;

    Dictionary<Vector2, Tile> tiles;
    Tile[,] objectPools;
    Camera cam;

    private void Start()
    {
        cam = MoveCame.editCam;
        GenerateGrid();
    }
    private void Update()
    {
        if (cam.gameObject.GetComponent<MoveCame>().GetCamEdgesArr[(int)Direction.Up] >= height)
        {
           // PoolManager.instance.SpawnFromPool();
        }
    }
    void GenerateGrid()
    {
        tiles = new Dictionary<Vector2, Tile>();
        PoolManager.instance.SetPoolMaxSize(width);
        objectPools = new Tile[width,height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var spawnedTile = Instantiate(tilePrefab,new Vector3(x,y), Quaternion.identity, gridSys.transform);
                spawnedTile.name = $"tile {x} {y}";
                PoolManager.instance.FillObjectPool(spawnedTile);
                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.init(isOffset);
                objectPools[x, y] = spawnedTile;
                tiles[new Vector2(x, y)] = spawnedTile;
            }
        }
       
        cam.transform.position = new Vector3((float)width / 2 - 0.05f, (float)height / 2 - 0.05f, -10);
     
    }
    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (tiles[pos]) return tiles[pos];
        else return null;
    }

}
