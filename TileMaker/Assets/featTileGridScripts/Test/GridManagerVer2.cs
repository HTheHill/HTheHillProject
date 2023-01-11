using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GridManagerVer2 : MonoBehaviour
{
    [SerializeField] private int width, height;
    [SerializeField] TestTile tilePrefab;
    [SerializeField] GameObject gridSys;

    Dictionary<Vector2, TestTile> tiles;
    TestTile[,] visibleTileMap;
    int[] mapSize;
    Camera cam;
    int top,down,right,left;

    [SerializeField] private string fileName;
    private int[,] mapAxis;
    private EditMode editMode;
    private void Start()
    {
        cam = MoveCame.editCam;
        FileReader();
        GenerateGrid();

    }
    private void OnEnable()
    {
        editMode = FindObjectOfType<EditMode>();
    }
    private void Update()
    {
        /*when camera's sight is bigger than map's height
         1. than get tiles from poolManger for one row
         2. set position for each tile gameobject to its 
         */

        if (cam.gameObject.GetComponent<MoveCame>().GetCamEdgesArr[(int)Direction.Up] >= top)
        {
          ShiftToUp(); 
          // csv check
        }
        else if (cam.gameObject.GetComponent<MoveCame>().GetCamEdgesArr[(int)Direction.Down] < down)
        {
            print("camEdge"+cam.gameObject.GetComponent<MoveCame>().GetCamEdgesArr[(int)Direction.Down]);
            ShiftDown();
        }
        else if (cam.gameObject.GetComponent<MoveCame>().GetCamEdgesArr[(int)Direction.Left] < left)
        {
            print("camEdge"+cam.gameObject.GetComponent<MoveCame>().GetCamEdgesArr[(int)Direction.Left]);
            ShiftLeft();
        }
        else if (cam.gameObject.GetComponent<MoveCame>().GetCamEdgesArr[(int)Direction.Right] >= right)
        {
            ShiftRight();
        }
    }
    private void FileReader()
    {
        mapAxis = new int[height, width];
        StreamReader sr = new StreamReader(Application.dataPath + "/Data/" + fileName);

        for (int y = 0; y < height; y++)
        {
            string fileData = sr.ReadLine();
            int[] data = Array.ConvertAll(fileData.Split(','), int.Parse);
            
            for (int x = 0; x < width; x++)
            {
                mapAxis[y, x] = data[x];
            }
        }
    }
    void GenerateGrid()
    {
        SetDirectionValue();
        tiles = new Dictionary<Vector2, TestTile>();
        PoolManagerVer2.instance.SetPoolMaxSize(width);
        visibleTileMap = new TestTile[width,height];
        int size = width * height * 2;
        for (int i = 0; i < size; i++)
        {
            var tile = Instantiate(tilePrefab, gridSys.transform);
            PoolManagerVer2.instance.FillObjectPool(tile);
        }
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
               var spawnedTile = PoolManagerVer2.instance.SpawnFromPool();
               spawnedTile.transform.position = new Vector2(y, x);
                // var spawnedTile = Instantiate(tilePrefab,new Vector3(y,x), Quaternion.identity, gridSys.transform);
                spawnedTile.name = $"tile {x} {y}";
                // print(spawnedTile.name);
               DrawColor(x,y,spawnedTile);
                visibleTileMap[x,y] = spawnedTile;
                
                tiles[new Vector2(y,x)] = spawnedTile;
            }
        }
        //changed line from newHj
        transform.position = new Vector3(-(width / 2), -(height / 2), 0);
       
        cam.transform.position = new Vector3((float)width / 2 - 0.05f, (float)height / 2 - 0.05f, -10);
     
    }

    public void DrawColor(int x, int y,TestTile spawnedTile)
    {
        x = Mathf.Abs(x);
        y = Mathf.Abs(y);
        var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
        spawnedTile.init(isOffset,mapAxis[x,y]);
    }

    public void SetDirectionValue()
    {
        top = height-1;
        down = 0;
        left = 0;
        right = width-1;
    }
    public TestTile GetTileAtPosition(Vector2 pos)
    {
        if (tiles[pos]) return tiles[pos];
        else return null;
    }

    public void ShiftToUp()
    {
        for(int i = 0; i < height; i++)
        {
            PoolManagerVer2.instance.GiveBackToPool(visibleTileMap[0,i]);
            print(visibleTileMap[0,i]);
            // this code will eventually throw index out of range 
        }

        for (int i = 1; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                visibleTileMap[i - 1, j] = visibleTileMap[i, j];
            }
        }
        top++;
        down++;
        for (int i = 0; i < width; i++)
        {
            TestTile newTile =  PoolManagerVer2.instance.SpawnFromPool();
            newTile.gameObject.SetActive(true);
            newTile.transform.position = new Vector2(i+left,top);
            DrawColor(i+left,top,newTile);
            visibleTileMap[height-1,i] = newTile;
            // print(newTile.transform); // becase it called many times 
            /*
             * 
             * height + 1 should be current top height +1 ;
             p1. it called many time 
             */
        }
    }

    public void ShiftDown()
    {
        //p1 y =0 is disapear for some reason;
        for(int i = height-1; i>=0; i--)
        {
            PoolManagerVer2.instance.GiveBackToPool(visibleTileMap[height-1,i]);
            // this code will eventually throw index out of range 
        }
        
        for (int i = height-2; i >=0; i--)
        {
            print(i);
            for (int j = 0; j < height; j++)
            {
                visibleTileMap[i + 1, j] = visibleTileMap[i, j];
            }
        }
        down--;
        top--;
        for (int i = 0; i < width; i++)
        {
            TestTile newTile =  PoolManagerVer2.instance.SpawnFromPool();
            print(newTile.gameObject.name);
            newTile.gameObject.SetActive(true);
            DrawColor(i+left,down, newTile);
            newTile.transform.position = new Vector2(i+left,down);
            // print(objectPools[0,i].gameObject.name);
            visibleTileMap[0,i] = newTile;
            newTile.gameObject.name = $"tile {0} {i}";
        }
    }

    public void ShiftLeft()
    {
        for(int i = 0; i < height; i++)
        {
            print($"<{width-1} {i}>");
            PoolManagerVer2.instance.GiveBackToPool(visibleTileMap[i,width-1]);
            // this code will eventually throw index out of range 
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = width-2; j >= 0; j--)
            {
                print(i+", "+j);
                visibleTileMap[i, j+1] = visibleTileMap[i, j];
            }
        }
        left--;
        right--;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (j == 0)
                {
                    TestTile newTile =  PoolManagerVer2.instance.SpawnFromPool();
                    newTile.gameObject.SetActive(true);
                    newTile.transform.position = new Vector2(left,i + down);
                    DrawColor(left,i + down,newTile);
                    visibleTileMap[i,j] = newTile;
                }
            }
            // print(newTile.transform); // becase it called many times 
            /*
             * 
             * height + 1 should be current top height +1 ;
             p1. it called many time 
             */
        }
    }

    public void ShiftRight()
    {
        for(int i = 0; i < height; i++)
        {
            print($"<{width-1} {i}>");
            PoolManagerVer2.instance.GiveBackToPool(visibleTileMap[i,0]);
            // this code will eventually throw index out of range 
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 1; j <width ; j++)
            {
                visibleTileMap[i, j-1] = visibleTileMap[i, j];
            }
        }
        
        left++;
        right++;
        StringBuilder sb = new StringBuilder();
        StringBuilder sb2 = new StringBuilder();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (j == width - 1)
                {
                    TestTile newTile =  PoolManagerVer2.instance.SpawnFromPool();
                    newTile.gameObject.SetActive(true);
                    newTile.transform.position = new Vector2(right,i+ down);
                    sb.Append($"<{right} {i + down}> ");
                    sb2.Append($"<{width - 1} {i}> ");
                    DrawColor(right,i+down,newTile);
                    visibleTileMap[i,j] = newTile; // 이놈
                }
              
            }
            // print(newTile.transform); // becase it called many times 
            /*
             * 
             * height + 1 should be current top height +1 ;
             p1. it called many time 
             */
        }

        // for (int i = 0; i < width; i++)
        // {
        //     StringBuilder sd = new StringBuilder();
        //     for (int j = 0; j < height; j++)
        //     {
        //         sd.Append(visibleTileMap[i, j].transform.position);
        //     }
        //     print(sd.ToString());
        // }
        print(sb.ToString());
        print(sb2.ToString());
        
    }

}
