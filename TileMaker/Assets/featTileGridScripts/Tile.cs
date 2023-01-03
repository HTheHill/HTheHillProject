using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] Color baseColor, offsetColor;
    [SerializeField] SpriteRenderer renderer;
    [SerializeField] GameObject highlights;

    public void init(bool offset)
    {
        renderer.color = offset? baseColor : offsetColor;
        
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
