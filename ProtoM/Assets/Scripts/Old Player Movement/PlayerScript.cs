using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    public int playerHealth = 28;
    public currentWeapon currentWeapon;

    Animator _animator;
    Rigidbody2D _rigidbody;
    Transform _transform;

    public LayerMask groundLayer;

    public int direction = 1;

    public float moveSpeed;

    public int jumpHeight;

    public float slideTime;

    public float shootTime;

    public bool canMove;
    public bool isGrounded;
    public bool isMoving;
    public bool isRunning;
    public bool isSliding;
    public bool isIdleShooting;
    public bool isShooting;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();

        canMove = true;
    }

    void Update()
    {
        Debug.Log(isIdleShooting);
    }

}
