using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestTile : MonoBehaviour
{
    [SerializeField] Color baseColor, offsetColor;
    
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private GameObject highlights;

    private TestGridManager gridManager;

    private void OnEnable()
    {
        gridManager = FindObjectOfType<TestGridManager>();
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        highlights = transform.GetChild(0).gameObject;
    }

    public void init(bool offset, int spriteIndex)
    {
        if (spriteIndex == 0)
        {
            spriteRenderer.color = offset? baseColor : offsetColor;
        }
        else
        {
            spriteRenderer.sprite = gridManager.tileSprites[spriteIndex];
            boxCollider.isTrigger = false;
        }
    }
    private void OnMouseEnter()
    {
        highlights.SetActive(true);
    }
    private void OnMouseExit()
    {
        highlights.SetActive(false);
    }
}
