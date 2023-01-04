using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Arrow : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    bool isPressed;
    MoveCame mvCam;
    Direction dir;
   

    public void OnPointerDown(PointerEventData eventData)
    {
        string[] dirArr = Enum.GetNames(typeof(Direction));
        for (int i = 0; i < dirArr.Length; i++)
        {
            if (dirArr[i].Equals(eventData.selectedObject.name))
            {
                dir = (Direction)i; break;
            }
        }
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
            mvCam.MoveCam(dir);
    }

   
}
