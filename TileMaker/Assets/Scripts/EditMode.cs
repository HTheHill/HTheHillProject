using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 

public enum GameMode
{
    Play,
    Edit
}

public class EditMode : MonoBehaviour
{
    [SerializeField] private List<GameObject> tilePrefabs;
    [SerializeField] private List<GameObject> tiles;

    private Dictionary<string, GameObject> tilePrefabDic = new Dictionary<string, GameObject>();
    private Stack<ICommand> commands = new Stack<ICommand>();

    [HideInInspector] public bool isSetting;
    private bool useEraser;
    private string selectTileType;
    private string noTileMessege = "no tile";

    private Button eraserButton;
    private Toggle tgSolid;
    private Toggle tgFire;
    private Toggle tgGoalFlag;
    private Toggle tgEraser;
    private Toggle tgUndo;

    void Awake()
    {
        selectTileType = "Solid";
        SetTilePrefabDictionary();
        
        InitToggle();
        
        tiles = new List<GameObject>();
        FindObjectOfType<InitMapSetting>().enabled = true;
    }

    private void InitToggle()
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

    private void SetTilePrefabDictionary()
    {
        foreach (var tilePrefab in tilePrefabs)
        {
            tilePrefabDic.Add(tilePrefab.name, tilePrefab);
        }
    }

    void Update()
    {
        AddTile();

        if (useEraser)
        {
            TileEraser();
        }
    }

    private void AddTile()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePoint.z = 0f;

                DeleteCommand deleteCommand = new DeleteCommand();
                deleteCommand.Execute(this, mousePoint);
                if(deleteCommand.TileType != noTileMessege) commands.Push(deleteCommand);

                AddCommand addCommand = new AddCommand();
                addCommand.Execute(this, mousePoint, selectTileType);
                commands.Push(addCommand);
            }
        }
    }

    public void CreateTile(Vector3 mousePoint, string tileType)
    {
        GameObject tile = Instantiate(tilePrefabDic[tileType], mousePoint, Quaternion.identity, gameObject.transform);
        Type type = Type.GetType(tileType);
        object instance = Activator.CreateInstance(type, GameMode.Edit, mousePoint);
        tile.GetComponent<TileControl>().SetTileType(instance as Tile);

        tiles.Add(tile);
    }

    private void TileEraser()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePoint.z = 0f;
                
                DeleteCommand deleteCommand = new DeleteCommand();
                deleteCommand.Execute(this, mousePoint);
                if(deleteCommand.TileType != noTileMessege) commands.Push(deleteCommand);
            }
        }
    }

    public string DeleteTile(Vector3 mousePoint)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePoint, Vector2.zero);

        foreach (var hit in hits)
        {
            if (hit.transform.CompareTag("Tile"))
            {
                GameObject tile = hit.transform.gameObject;
                Tile tileType = tile.GetComponent<TileControl>().GetTileType();

                tiles.Remove(tile);
                Destroy(tile);

                return tileType.TileName;
            }
        }

        return noTileMessege;
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
