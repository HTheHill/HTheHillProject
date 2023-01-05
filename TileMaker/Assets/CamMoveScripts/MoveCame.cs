using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public enum Direction
{
    Up,
    Down,
    Left,
    Right
}


public class MoveCame : MonoBehaviour
{
    public static Camera editCam;
    static MoveCame instance;
    public static MoveCame GetMoveCam { get { return instance; } }

    public float zoomSpeed = 0.5f;
    public float minZoom = 10f;
    public float maxZoom = 100f;
    [SerializeField] Button upArrow;
    bool isPressed;
    Vector3[] vecArray = new Vector3[4] { Vector3.up, Vector3.down, Vector3.left, Vector3.right };

    
    public float camMoveSpeed = 0.5f;
    private void Awake()
    {
        instance = this;
    }
    private void OnEnable()
    {
        if (!editCam)
        {
            editCam = Camera.main;
        }
    }
 

    // Update is called once per frame
    void Update()
    {

        ZoomInOut();
    }
    void ZoomInOut()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            Camera.main.orthographicSize -= zoomSpeed;

            // Limit zoom
            if (Camera.main.orthographicSize < minZoom)
            {
                Camera.main.orthographicSize = minZoom;
            }
        }
        // Zoom out
        else if (scroll < 0f)
        {
            Camera.main.orthographicSize += zoomSpeed;

            // Limit zoom
            if (Camera.main.orthographicSize > maxZoom)
            {
                Camera.main.orthographicSize = maxZoom;
            }
        }
    }

    #region CameraPositionMove
    
    public void MoveCam(Direction dir)
    {
        transform.Translate(vecArray[(int)dir] * Time.deltaTime * editCam.orthographicSize * camMoveSpeed);
    }

    public void MoveUp()
    {
        transform.Translate(Vector3.up * Time.deltaTime * editCam.orthographicSize * camMoveSpeed);
    }
    public void MoveDown()
    {
        transform.Translate(Vector3.down * Time.deltaTime * editCam.orthographicSize * camMoveSpeed);
    }
    public void MoveLeft()
    {
        transform.Translate(Vector3.left * Time.deltaTime * editCam.orthographicSize * camMoveSpeed);
    }
    public void MoveRight()
    {
        transform.Translate(Vector3.right * Time.deltaTime * editCam.orthographicSize * camMoveSpeed);
    }
    #endregion



}
