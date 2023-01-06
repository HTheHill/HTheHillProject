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

    private int[,] mapAxis;
    private Camera cam;
    private EditMode editMode;

    private void OnEnable()
    {
        editMode = FindObjectOfType<EditMode>();
    }

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
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                TestTile spawnedTile = Instantiate(tilePrefab,new Vector3(x, y,0), Quaternion.identity, transform);
                spawnedTile.name = $"tile {x} {y}";

                bool isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.init(isOffset, mapAxis[y, x]);

                editMode.tiles[new Vector2(x, y)] = spawnedTile;
            }
        }
        // Change grid position to center
        transform.position = new Vector3(-(width / 2), -(height / 2), 0);

        cam = MoveCame.editCam;
        cam.transform.position = new Vector3((float)width / 2 - 0.05f, (float)height / 2 - 0.05f, -10);
    }

    public TestTile GetTileAtPosition(Vector2 pos)
    {
        if (editMode.tiles[pos]) return editMode.tiles[pos];
        else return null;
    }
}
