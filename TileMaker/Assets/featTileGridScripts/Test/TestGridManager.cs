using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class TestGridManager : MonoBehaviour
{
    [SerializeField] private string fileName;
    [SerializeField] private int width, height;
    [SerializeField] private TestTile tilePrefab;
    public Sprite[] tileSprites;

    private int[,] mapAxis;
    Dictionary<Vector2, TestTile> tiles;
    Camera cam;
    
    private void Start()
    {
        FileReader();
        GenerateGrid();
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

        // TestFileReader();
    }

    void TestFileReader()
    {
        for (int i = 0; i < mapAxis.GetLength(0); i++)
        {
            for (int j = 0; j < mapAxis.GetLength(1); j++)
            {
                Debug.Log("axis : " + i + "," + j + " : " + mapAxis[i, j]);
            }
        }
    }
    
    void GenerateGrid()
    {
        tiles = new Dictionary<Vector2, TestTile>();   

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                TestTile spawnedTile = Instantiate(tilePrefab,new Vector3(x, y,0), Quaternion.identity, transform);
                spawnedTile.name = $"tile {x} {y}";

                bool isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                Debug.Log(isOffset);
                spawnedTile.init(isOffset, mapAxis[y, x]);

                tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        transform.position = new Vector3(-(width / 2), -(height / 2), 0);
        
        cam = MoveCame.editCam;
        cam.transform.position = new Vector3((float)width / 2 - 0.05f, (float)height / 2 - 0.05f, -10);
    }
    
    public TestTile GetTileAtPosition(Vector2 pos)
    {
        if (tiles[pos]) return tiles[pos];
        else return null;
    }
}
