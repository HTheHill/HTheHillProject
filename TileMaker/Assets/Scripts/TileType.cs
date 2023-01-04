using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileType
{
    private string tileName;
    private GameMode gameMode;
    
    public string TileName { get { return tileName; } }
    public GameMode GameMode { get { return gameMode; } }

    public TileType(string tileName, GameMode gameMode = GameMode.Edit)
    {
        this.tileName = tileName;
        this.gameMode = gameMode;
    }
    
    public abstract void Skill();
}

public class Solid : TileType
{
    public Solid() : base("Solid") { }

    public override void Skill()
    {
        Debug.Log("Solid!");
    }
}

public class Fire : TileType
{
    public Fire() : base("Fire") { }

    public override void Skill()
    {
        Debug.Log("Fire!");
    }
}

public class GoalFlag : TileType
{
    public GoalFlag() : base("GoalFlag") { }
    
    public override void Skill()
    {
        Debug.Log("Goal!");
    }
}