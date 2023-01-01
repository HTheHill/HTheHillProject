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

    private bool useEraser;
    private string selectTileType;

    private Button eraserButton;
    private Toggle tgSolid;
    private Toggle tgFire;
    private Toggle tgEraser;
    private Toggle tgUndo;

    void Awake()
    {
        selectTileType = "Solid";
        SetTilePrefabDictionary();
        
        tiles = new List<GameObject>();
        InitMap();
        
        InitToggle();
    }
    
    private void InitMap()
    {
        if (tilePrefabDic.Count == 0) SetTilePrefabDictionary();
        
        GameObject[] tileObjects = GameObject.FindGameObjectsWithTag("Tile");
        foreach (var tile in tileObjects)
        {
            string tileType = FindTileType(tile.name);
            
            Type type = Type.GetType(tileType);
            object instance = Activator.CreateInstance(type, GameMode.Edit, tile.transform.position);
            tile.GetComponent<TileControl>().SetTileType(instance as Tile);
            
            tiles.Add(tile);
        }
    }

    private string FindTileType(string tileName)
    {
        foreach (var tilePrefabName in tilePrefabDic.Keys)
        {
            if (tileName.Contains(tilePrefabName))
            {
                return tilePrefabName;
            }
        }
        
        return selectTileType == null ? "Solid" : selectTileType;
    }

    private void InitToggle()
    {
        tgSolid = GameObject.Find("Solid Toggle").GetComponent<Toggle>();
        tgSolid.onValueChanged.AddListener(delegate { selectTileType = "Solid"; });
        tgFire = GameObject.Find("Fire Toggle").GetComponent<Toggle>();
        tgFire.onValueChanged.AddListener(delegate { selectTileType = "Fire"; });
        
        tgEraser = GameObject.Find("Eraser Toggle").GetComponent<Toggle>();
        tgEraser.onValueChanged.AddListener(delegate { useEraser = !useEraser; });
        tgUndo = GameObject.Find("Undo Toggle").GetComponent<Toggle>();
        tgUndo.onValueChanged.AddListener(delegate {  });
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
                DeleteTile(mousePoint);

                mousePoint.z = 0f;
                
                GameObject tile = Instantiate(tilePrefabDic[selectTileType], mousePoint, Quaternion.identity);
                Type type = Type.GetType(selectTileType);
                object instance = Activator.CreateInstance(type, GameMode.Edit, mousePoint);
                tile.GetComponent<TileControl>().SetTileType(instance as Tile);
                
                tiles.Add(tile);
            }
        }
    }

    private void TileEraser()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                DeleteTile(mousePoint);
            }
        }
    }

    private void DeleteTile(Vector3 mousePoint)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePoint, Vector2.zero);

        foreach (var hit in hits)
        {
            if (hit.transform.CompareTag("Tile"))
            {
                GameObject tile = hit.transform.gameObject;

                tiles.Remove(tile);
                Destroy(tile);
            }
        }
    }
}