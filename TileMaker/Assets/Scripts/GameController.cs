using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AI;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private PlayerMove playerMove;
    public bool isGameOver;
    public bool isPlaying;

    void Start()
    {
        isGameOver = false;
        isPlaying = true;
        GameObject Player = Instantiate(player, new Vector3(0, 2, 0), Quaternion.identity);
        playerMove = Player.GetComponent<PlayerMove>();
    }

    public void OnPlayerWin()
    {
        Debug.Log("결승점 도달!");
        isGameOver = true;
    }

    public void OnPlayerDead()
    {
        Debug.Log("플레이어 죽음");
        isGameOver = true;
    }
    public void OnEditBtnClick()
    {
        isPlaying = false;
        playerMove.enabled = false;
    }

    public void OnPlayBtnClick()
    {
        isPlaying = true;
        playerMove.enabled = true;
    }
}
