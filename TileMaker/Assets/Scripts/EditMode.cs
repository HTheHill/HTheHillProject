using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 

public enum GameMode
{
    Play,
    Edit
}

public enum TileNum
{
    Default,
    Solid,
    Fire,
    GoalFlag
}

public class EditMode : MonoBehaviour
{
    public Sprite[] displayTileSprites;
    public Sprite[] higlightTileSprties;
    public Sprite[] otherSprites;
    
    [HideInInspector] public Stack<ICommand> commands = new Stack<ICommand>();
    [HideInInspector] public Dictionary<Vector2, TestTile> tiles = new Dictionary<Vector2, TestTile>();
    
    private bool useEraser;
    public bool UseEraser { get { return useEraser; } }
    private string selectTileType;
    public string SelectTileType { get { return selectTileType; } }

    private Toggle tgSolid;
    private Toggle tgFire;
    private Toggle tgGoalFlag;
    private Toggle tgEraser;
    private Toggle tgUndo;

    private void OnEnable()
    {
        selectTileType = "Solid";
        InitToggle();
    }

    public void InitToggle()
    {
        tgSolid = GameObject.Find("Solid Toggle").GetComponent<Toggle>();
        tgSolid.onValueChanged.AddListener(delegate { selectTileType = "Solid"; });
        tgFire = GameObject.Find("Fire Toggle").GetComponent<Toggle>();
        tgFire.onValueChanged.AddListener(delegate { selectTileType = "Fire"; });
        tgGoalFlag = GameObject.Find("GoalFlag Toggle").GetComponent<Toggle>();
        tgGoalFlag.onValueChanged.AddListener(delegate { selectTileType = "GoalFlag"; });
        
        tgEraser = GameObject.Find("Eraser Toggle").GetComponent<Toggle>();
        tgEraser.onValueChanged.AddListener(delegate { useEraser = !useEraser; });
        tgUndo = GameObject.Find("Undo Toggle").GetComponent<Toggle>();
        tgUndo.onValueChanged.AddListener(delegate { WorkUndo(); });
    }

    private void WorkUndo()
    {
        if (commands.Count == 0)
        {
            Debug.Log("commands.Count : " + commands.Count);
            return;
        }

        ICommand command = commands.Pop();
        command.Undo(this);
    }
}
