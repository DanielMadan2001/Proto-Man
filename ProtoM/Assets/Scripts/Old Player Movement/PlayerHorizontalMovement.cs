using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHorizontalMovement : MonoBehaviour
{
    public PlayerScript playerScript;

    Animator _animator;
    Rigidbody2D _rigidbody;
    Transform _transform;

    public Transform groundCheck;
    public Transform groundCheck2;
    private float moveTime = 0;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
    }

    void Update()
    {
        if (playerScript.canMove && !playerScript.isIdleShooting)
        {
            GroundCheck();
            DirectionCheck();
            Moving();
        }
    }

    void GroundCheck()
    {
        bool check1 = Physics2D.Linecast(_transform.position, groundCheck.position, playerScript.groundLayer);
        bool check2 = Physics2D.Linecast(_transform.position, groundCheck2.position, playerScript.groundLayer);

        playerScript.isGrounded = check1;

        _animator.SetBool("isGrounded", playerScript.isGrounded);
    }

    void DirectionCheck()
    {
        Vector3 localScale = _transform.localScale;
        if ((Input.GetAxisRaw("Horizontal") == 1 && playerScript.direction == -1) || (Input.GetAxisRaw("Horizontal") == -1 && playerScript.direction == 1))
        {
            playerScript.direction *= -1;
        }
        localScale.x = playerScript.direction;
        _transform.localScale = localScale;
    }

    void Moving()
    {
        MoveCheck();
        RunCheck();
        Running();
    }

    void MoveCheck()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
           playerScript.isMoving = true;
           moveTime += Time.deltaTime;
        }
        else
        {
            playerScript.isMoving = false;
            moveTime = 0;
        }

        _animator.SetBool("isMoving", playerScript.isMoving);
    }

    void RunCheck()
    {
        if (moveTime > 0.085f)
        {
            playerScript.isRunning = true;
        }
        else
        {
            playerScript.isRunning = false;
        }

        _animator.SetBool("isRunning", playerScript.isRunning);
    }

    void Running()
    {
        if (!playerScript.isSliding)
        {
            if (playerScript.isRunning || !playerScript.isGrounded)
            {
                _rigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * playerScript.moveSpeed, _rigidbody.velocity.y);
            }
            else
            {
                _rigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * (playerScript.moveSpeed * 0.2f), _rigidbody.velocity.y);
            }
        }
    }
}
