using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestTile : MonoBehaviour
{
    [SerializeField] Color baseColor, offsetColor;

    private int tileIndex;
    public int TileIndex{ get { return tileIndex; } }
    private Color initColor;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private GameObject highlights;
    private SpriteRenderer highlightRenderer;
    
    private EditMode editMode;

    private void OnEnable()
    {
        editMode = FindObjectOfType<EditMode>();
        
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        highlights = transform.GetChild(0).gameObject;
        highlightRenderer = highlights.GetComponent<SpriteRenderer>();
    }

    public void init(bool offset, int spriteIndex)
    {
        tileIndex = spriteIndex;
        initColor = offset? baseColor : offsetColor;
        
        SetTileType(tileIndex);
    }
    public void init(bool offset)
    {
        spriteRenderer.color = offset? baseColor : offsetColor;
    }
    private void OnMouseEnter()
    {
        if (editMode.GameMode == GameMode.Play || EventSystem.current.IsPointerOverGameObject()) return;

        highlights.SetActive(true);
        if (editMode.UseEraser)
        {
            // set eraser sprite
            highlightRenderer.sprite = editMode.otherSprites[0];
        }
        else
        {
            int spriteIndex = (int)Enum.Parse(typeof(TileNum), editMode.SelectTileType);
            highlightRenderer.sprite = editMode.higlightTileSprties[spriteIndex];
        }
    }
    private void OnMouseExit()
    {
        highlightRenderer.sprite = editMode.higlightTileSprties[0];
        highlights.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (editMode.GameMode == GameMode.Play || EventSystem.current.IsPointerOverGameObject()) return;

        if (editMode.UseEraser && spriteRenderer.sprite != editMode.displayTileSprites[0])
        {
            Command command = new Command();
            command.Execute(this, 0);
            editMode.commands.Push(command);
        }
        else if(!editMode.UseEraser)
        {
            int spriteIndex = (int)Enum.Parse(typeof(TileNum), editMode.SelectTileType);
            Command command = new Command();
            command.Execute(this, spriteIndex);
            editMode.commands.Push(command);
        }
    }

    public void SetTileType(int spriteIndex)
    {
        tileIndex = spriteIndex;
        
        if (tileIndex == 0)
        {
            spriteRenderer.color = initColor;
            boxCollider.isTrigger = true;
        }
        else
        {
            spriteRenderer.color = Color.white;
            boxCollider.isTrigger = false;
        }
        
        spriteRenderer.sprite = editMode.displayTileSprites[tileIndex];
        gameObject.layer = GetLayerNumber();
    }
    
    private int GetLayerNumber()
    {
        string getLayerNum = Enum.GetName(typeof(TileNum), tileIndex);
        switch (getLayerNum)
        {
            case "Solid" :
                return LayerMask.NameToLayer("Ground"); 
            case "Fire" :
                return LayerMask.NameToLayer("Enemy");
            case "GoalFlag" :
                return LayerMask.NameToLayer("GoalFlag");
        }
        
        return LayerMask.NameToLayer("Default");
    }
}
