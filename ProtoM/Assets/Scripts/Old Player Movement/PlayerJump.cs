using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public PlayerScript playerScript;

    Rigidbody2D _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (playerScript.canMove && !playerScript.isIdleShooting)
        {
            Jump();
        }
    }

    void Jump()
    {
        float vy = _rigidbody.velocity.y;
        if (Input.GetButtonDown("Jump") && playerScript.isGrounded && Input.GetAxisRaw("Vertical") > -1 && !playerScript.isSliding)
        {
            vy = 0f;
            _rigidbody.AddForce(new Vector2(0, playerScript.jumpHeight));
        }

        if (Input.GetButtonUp("Jump") && vy > 0)
        {
            _rigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * playerScript.moveSpeed, -1f);
        }
    }
}
