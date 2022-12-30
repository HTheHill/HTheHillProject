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

    void Awake()
    {
        tiles = new List<GameObject>();
        InitMap();
        
        selectTileType = "Solid";
        InitToggle();

        SetTilePrefabDictionary();
    }
    
    private void InitMap()
    {
        GameObject[] tileObjects = GameObject.FindGameObjectsWithTag("Tile");
        foreach (var tile in tileObjects)
        {
            Solid solid = new Solid(GameMode.Play, tile.transform.position);
            tile.GetComponent<TileControl>().SetTileType(solid);
            tiles.Add(tile);
        }
    }

    private void InitToggle()
    {
        eraserButton = GameObject.Find("Eraser Button").GetComponent<Button>();
        eraserButton.onClick.AddListener(OnUseEraser);
        tgSolid = GameObject.Find("Solid Toggle").GetComponent<Toggle>();
        tgSolid.onValueChanged.AddListener(delegate { selectTileType = "Solid"; });
        tgFire = GameObject.Find("Fire Toggle").GetComponent<Toggle>();
        tgFire.onValueChanged.AddListener(delegate { selectTileType = "Fire"; });
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

    private void OnUseEraser()
    {
        useEraser = !useEraser;
        Debug.Log("Use Eraser : " + useEraser);
    }
}
