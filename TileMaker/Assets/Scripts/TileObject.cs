using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MonoBehaviour
{
    private GameMode gameMode;
    private Vector3 initPosition;
    private TileType tileType;
    
    public GameMode GameMode { get { return gameMode; } }
    public Vector3 InitPosition { get { return initPosition; } }
    public TileType TileType { get { return tileType; } }

    public void CreateTile(GameMode gameMode, Vector3 initPosition, Type type)
    {
        tileType = Activator.CreateInstance(type) as TileType;
        this.initPosition = initPosition;
        this.gameMode = gameMode;
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