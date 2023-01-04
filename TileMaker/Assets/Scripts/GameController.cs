using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    public bool isGameOver;

    void Start()
    {
        isGameOver = false;
        Instantiate(player, new Vector3(0, 2, 0), Quaternion.identity);
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
}
