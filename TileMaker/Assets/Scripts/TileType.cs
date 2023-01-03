using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileType
{
    private string tileName;
    public string TileName { get { return tileName; } }

    public TileType(string tileName)
    {
        this.tileName = tileName;
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