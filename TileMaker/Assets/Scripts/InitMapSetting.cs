using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public struct MapInfo
{
    private int id;
    private int axis;
    private string tileType;
    
    public int Id { get { return id; } }
    public int Axis { get { return axis; } }
    public string TileType { get { return tileType; } }

    public MapInfo(int id, int axis, string tileType)
    {
        this.id = id;
        this.axis = axis;
        this.tileType = tileType;
    }
}

public class InitMapSetting : MonoBehaviour
{
    [SerializeField] private string fileName;
    [SerializeField] private int mapWidth = 20;
    
    private List<MapInfo> mapInfos = new List<MapInfo>();
    private void OnEnable()
    {
        FileReader();
        DrawMap();
    }

    private void FileReader()
    {
        StreamReader sr = new StreamReader(Application.dataPath + "/Data/" + fileName);

        bool endOfFile = false;
        string headerData = sr.ReadLine();
        
        while (!endOfFile)
        {
            // id axis tile
            string fileData = sr.ReadLine();

            if (fileData == null)
            {
                endOfFile = true;
                break;
            }

            string[] data = fileData.Split(',');
            mapInfos.Add(new MapInfo(int.Parse(data[0]), int.Parse(data[1]), data[2]));
        }
    }

    private void DrawMap()
    {
        if(mapInfos.Count == 0) FileReader();
        EditMode editMode = FindObjectOfType<EditMode>();

        editMode.isSetting = true;
        
        int index = 0, mapX = 0, mapY = 0;
        while (index < mapInfos.Count)
        {
            if (mapInfos[index].Axis == 1)
            {
                mapY = mapInfos[index].Id / mapWidth;
                mapX = mapInfos[index].Id - (mapY * mapWidth);
            
                editMode.CreateTile(new Vector3(mapX, mapY, 0f), mapInfos[index].TileType);
            }

            index++;
        }
        
        editMode.isSetting = false;
        this.enabled = false;
    }
}
