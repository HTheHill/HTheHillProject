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
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private List<GameObject> tiles;

    private bool useEraser;

    private Button eraserButton;

    void Awake()
    {
        tiles = new List<GameObject>();
        InitMap();

        eraserButton = GameObject.Find("Eraser Button").GetComponent<Button>();
        eraserButton.onClick.AddListener(OnUseEraser);
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

    void Update()
    {
        AddTile();

        if (useEraser)
        {
            TileEraser();
        }
    }

    void AddTile()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                DeleteTile(mousePoint);

                mousePoint.z = 0f;
                GameObject tile = Instantiate(tilePrefab, mousePoint, Quaternion.identity);
                Solid solid = new Solid(GameMode.Edit, mousePoint);
            
                tile.GetComponent<TileControl>().SetTileType(solid);
                tiles.Add(tile);
            }
        }
    }

    void TileEraser()
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

    void OnUseEraser()
    {
        useEraser = !useEraser;
        Debug.Log("Use Eraser : " + useEraser);
    }
}
