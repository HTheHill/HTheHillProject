using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WorkType 
{
    Add,
    Delete
}

public interface ICommand
{
    // public void Execute();
    public void Undo(EditMode editMode, Command command);
}

public class Command : ICommand
{
    private Tile tile;
    private Vector3 position;
    private WorkType workType;
    
    public Command(Tile tile, Vector3 position, WorkType workType)
    {
        this.tile = tile;
        this.position = position;
        this.workType = workType;
    }

    public void Undo(EditMode editMode, Command command)
    {
        switch (command.workType)
        {
            case WorkType.Add :
                editMode.DeleteTile(command.position);
                break;
            case WorkType.Delete :
                editMode.CreateTile(command.position, command.tile.TileName);
                break;
        }
    }
}
