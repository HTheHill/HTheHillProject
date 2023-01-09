using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
    public void Execute(TestTile tile, int tileIndex);
    public void Undo(EditMode editMode);
}

public class Command : ICommand
{
    private int preTileIndex;
    private Vector2 position;

    public void Execute(TestTile tile, int tileIndex)
    {
        position = tile.transform.localPosition;
        preTileIndex = tile.TileIndex;
        tile.SetTileType(tileIndex);
    }
    
    public void Undo(EditMode editMode)
    {
        editMode.tiles[position].SetTileType(preTileIndex);
    }
}
