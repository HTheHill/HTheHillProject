using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MonoBehaviour
{
    private string tileName;
    private GameMode gameMode;
    private Vector3 initPosition;
    private TileType tileType;
    
    public string TileName { get { return tileName; } }
    public GameMode GameMode { get { return gameMode; } set { gameMode = value; } }
    public Vector3 InitPosition { get { return initPosition; } }
    public TileType TileType { get { return tileType; } }

    public void CreateTile(Vector3 initPosition, Type type)
    {
        this.initPosition = initPosition;
        tileType = Activator.CreateInstance(type) as TileType;
        tileName = tileType.TileName;
        gameMode = tileType.GameMode;
    }

    public void DeleteTile()
    {
        Destroy(gameObject);
    }

    public void Skill()
    {
        tileType.Skill();
    }
}