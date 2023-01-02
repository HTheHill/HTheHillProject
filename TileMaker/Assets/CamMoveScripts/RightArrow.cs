using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RightArrow : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    bool isPressed;
    MoveCame mvCam;

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }
    void Start()
    {
        mvCam = MoveCame.GetMoveCam;
    }
    void Update()
    {
        if (isPressed)
            mvCam.MoveRight();
    }
}
