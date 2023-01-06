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
    int[] mapSize;
    Camera cam;
    int top;

    private void Start()
    {
        cam = MoveCame.editCam;
        GenerateGrid();

    }
    private void Update()
    {
        /*when camera's sight is bigger than map's height
         1. than get tiles from poolManger for one row
         2. set position for each tile gameobject to its 
         */

        if (cam.gameObject.GetComponent<MoveCame>().GetCamEdgesArr[(int)Direction.Up] >= height)
        {
            top++;
            for(int i = 0; i < height; i++)
            {
                PoolManager.instance.GiveBackToPool(objectPools[i, 0]);
                // this code will eventually throw index out of range 
            }
            for (int i = 0; i < width; i++)
            {
               Tile newTile =  PoolManager.instance.SpawnFromPool();
                newTile.gameObject.SetActive(true);
               newTile.transform.position = new Vector2(i,top);
                objectPools[i,0] = newTile;
                print(newTile.transform); // becase it called many times 
                /*
                 * 
                 * height + 1 should be current top height +1 ;
                 p1. it called many time 
                 */
            }

           // PoolManager.instance.SpawnFromPool();
            // dont know what to do think i should chang buttom row's coordinate to t
        }
    }
    void GenerateGrid()
    {
        top = height;
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
