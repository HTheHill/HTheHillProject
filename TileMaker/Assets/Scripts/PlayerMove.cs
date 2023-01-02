using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Vector2 JumpForce = new Vector2(0, 500);
    [SerializeField] private float speed = 6f;
    
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;

    private float xMove;
    private bool isGround;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        xMove = 0.0f;

        isGround = false;
    }
    
    private void FixedUpdate()
    {
        // Player를 따라오는 카메라
        Camera.main.transform.position =
            new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
        
    }

    void Update()
    {
        Movement();
        CheckGround();
        Jumping();
    }

    void Movement()
    {
        xMove = Input.GetAxis("Horizontal");
        rigidBody.velocity = new Vector2(speed * xMove, rigidBody.velocity.y);

        if (xMove != 0)
        {
            // xMove가 -1일 때는 SpriteRenderer를 뒤집는다.
            FlipX(xMove);
        }

    }
    void FlipX(float xMove) => spriteRenderer.flipX = xMove == -1;
    
    void CheckGround()
    {
        // 바닥 체크용 circle의 크기: collider의 반?? 이게 뭐였지
        float distance = (GetComponent<CapsuleCollider2D>().size.y / 2 *
                          transform.localScale.y) + .01f;

        isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(0, -distance), 0.07f,
            1 << LayerMask.NameToLayer("Ground"));
    }

    void Jumping()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGround)
            {
                rigidBody.velocity = Vector2.zero;
                rigidBody.AddForce(JumpForce);
            }
        }
    }
}
