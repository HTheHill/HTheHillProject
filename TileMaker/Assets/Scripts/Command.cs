using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
    public void Execute(EditMode editMode, Vector3 position, string tileType);
    public void Undo(EditMode editMode);
}

public class AddCommand : ICommand
{
    private Vector3 position;
    
    public void Execute(EditMode editMode, Vector3 position, string tileType)
    {
        this.position = position;
        editMode.CreateTile(this.position, tileType);
    }
    
    public void Undo(EditMode editMode)
    {
        editMode.DeleteTile(position);
    }
}

public class DeleteCommand : ICommand
{
    private Vector3 position;
    private string tileType;
    public string TileType { get { return tileType; } }
    
    public void Execute(EditMode editMode, Vector3 position, string tileType = "no tile")
    {
        this.position = position;
        this.tileType = editMode.DeleteTile(this.position);
    }
    
    public void Undo(EditMode editMode)
    {
        editMode.CreateTile(position, tileType);
    }
}
