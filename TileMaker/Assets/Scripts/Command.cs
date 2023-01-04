using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public interface ICommand
{
    public void Execute(EditMode editMode, Vector3 position, string tileName = "no tile");
    public void Undo(EditMode editMode);
}

public class AddCommand : ICommand
{
    private TileObject tileObject;
    public void Execute(EditMode editMode, Vector3 position, string tileName)
    {
        tileObject = editMode.CreateTile(position, tileName);
    }
    
    public void Undo(EditMode editMode)
    {
        editMode.DeleteTile(tileObject.InitPosition);
    }
}

public class DeleteCommand : ICommand
{
    [CanBeNull] private TileObject tileObject;
    public TileObject TileObject { get { return tileObject; } }

    public void Execute(EditMode editMode, Vector3 position, string tileName = "no tile")
    {
        tileObject = editMode.DeleteTile(position);
    }
    
    public void Undo(EditMode editMode)
    {
        editMode.CreateTile(tileObject.InitPosition, tileObject.TileName);
    }
}
