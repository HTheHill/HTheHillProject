using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileControl : MonoBehaviour
{
    private TileType tileType;

    public void SetTileType(TileType tileType)
    {
        this.tileType = tileType;
        this.tileType.Skill();
    }

    public TileType GetTileType()
    {
        return tileType;
    }
}
