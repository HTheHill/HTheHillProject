using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private bool changeMode;
    
    private string selectTileType;
    public string SelectTileType { get { return selectTileType; } }
    private GameMode gameMode;
    public GameMode GameMode { get { return gameMode; } }
    
    // private Toggle tgSolid;
    // private Toggle tgFire;
    // private Toggle tgGoalFlag;
    // private Toggle tgEraser;
    // private Toggle tgUndo;

    private GameController gameController;

    private void OnEnable()
    {
        gameMode = GameMode.Play;
        gameController = FindObjectOfType<GameController>();
    }

    private void Update()
    {
        if (!gameController.isPlaying && !changeMode)
        {
            changeMode = true;
            gameMode = GameMode.Edit;
            selectTileType = "Solid";
        }
        else if (gameController.isPlaying && changeMode)
        {
            changeMode = false;
            gameMode = GameMode.Play;
        }
    }

    // public void InitToggle()
    // {
    //     tgSolid = GameObject.Find("Solid Toggle").GetComponent<Toggle>();
    //     tgSolid.onValueChanged.AddListener(delegate { selectTileType = "Solid"; });
    //     tgFire = GameObject.Find("Fire Toggle").GetComponent<Toggle>();
    //     tgFire.onValueChanged.AddListener(delegate { selectTileType = "Fire"; });
    //     tgGoalFlag = GameObject.Find("GoalFlag Toggle").GetComponent<Toggle>();
    //     tgGoalFlag.onValueChanged.AddListener(delegate { selectTileType = "GoalFlag"; });
    //
    //     tgEraser = GameObject.Find("Eraser Toggle").GetComponent<Toggle>();
    //     tgEraser.onValueChanged.AddListener(delegate { useEraser = !useEraser; Debug.Log(useEraser); });
    //     tgUndo = GameObject.Find("Undo Toggle").GetComponent<Toggle>();
    //     tgUndo.onValueChanged.AddListener(delegate { WorkUndo(); });
    // }

    public void SelectToggle(int selectTileIndex)
    {
        string selectTile = Enum.GetName(typeof(TileNum), selectTileIndex);
        selectTileType = selectTile;
    }

    public void ChangeUseEraser()
    {
        useEraser = !useEraser;
    }

    public void WorkUndo()
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
