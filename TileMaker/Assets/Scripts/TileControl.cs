using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileControl : MonoBehaviour
{
    private Tile tileType;

    public void SetTileType(Tile tileType)
    {
        this.tileType = tileType;
        this.tileType.Skill();
    }

    public Tile GetTileType()
    {
        return tileType;
    }
}
