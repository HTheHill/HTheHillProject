using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    private GameController gameController;
    private PlayerMove pm;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        pm = GetComponent<PlayerMove>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Die();
        }

        if (col.gameObject.layer == LayerMask.NameToLayer("GoalFlag"))
        {
            Win();
        }    
    }

    void Die()
    {
        pm.enabled = false;
        gameController.OnPlayerDead();
        //Did 애니메이션 재생
    }

    void Win()
    {
        pm.enabled = false;
        gameController.OnPlayerWin();
        //Win 애니메이션 재생
    }
}
