using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public abstract class Tile
{
    private string tileName;
    private GameMode gameState;
    private Vector3 initPosition;

    public string TileName
    {
        get { return tileName; }
    } // get
    public GameMode GameState
    {
        get { return gameState; }
        set { gameState = value; }
    } // get set
    public Vector3 InitPosition
    {
        get { return initPosition; }
    } // get
    
    public Tile(string tileName, GameMode gameState, Vector3 initPosition)
    {
        this.tileName = tileName;
        this.gameState = gameState;
        this.initPosition = initPosition;
    }
    
    public abstract void Skill();
}

public class Solid : Tile
{
    public Solid(GameMode gameState, Vector3 initPosition) : base("Solid", gameState, initPosition) { }

    public override void Skill()
    {
        Debug.Log("Solid!");
    }
}

public class Fire : Tile
{
    public Fire(GameMode gameState, Vector3 initPosition) : base("Fire", gameState, initPosition) { }

    public override void Skill()
    {
        Debug.Log("Fire!");
    }
}

public class GoalFlag : Tile
{
    public GoalFlag(GameMode gameState, Vector3 initPosition) : base("GoalFlag", gameState, initPosition) { }
    
    public override void Skill()
    {
        Debug.Log("Goal!");
    }
}
