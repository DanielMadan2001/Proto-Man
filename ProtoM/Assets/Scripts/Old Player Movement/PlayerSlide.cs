using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlide : MonoBehaviour
{
    public PlayerScript playerScript;

    Animator _animator;
    Rigidbody2D _rigidbody;
    Transform _transform;

    bool cantStopSliding;
    public Transform slideCheck;
    public Transform slideCheck2;
    private float slideTimeLeft;
    private float slideSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();

        slideTimeLeft = playerScript.slideTime;
        slideSpeed = playerScript.moveSpeed * 2;
    }

    // Update is called once per frame
    void Update()
    {
        PressSlideButton();
        SlideCheck();
        ContinuousSlide();
    }

    void PressSlideButton()
    {
        if (Input.GetButtonDown("Jump") && playerScript.isGrounded && Input.GetAxisRaw("Vertical") == -1)
        {
            playerScript.isSliding = true;
        }
        _animator.SetBool("isSliding", playerScript.isSliding);
    }

    void SlideCheck()
    {
        if (Physics2D.Linecast(_transform.position, slideCheck.position, playerScript.groundLayer) || Physics2D.Linecast(_transform.position, slideCheck2.position, playerScript.groundLayer))
        {
            cantStopSliding = true;
        }
        else
        {
            cantStopSliding = false;
        }
    }

    void ContinuousSlide()
    {
        if (playerScript.isSliding && slideTimeLeft > 0 || cantStopSliding)
        {
            _rigidbody.velocity = new Vector2(playerScript.direction * slideSpeed, _rigidbody.velocity.y);
            slideTimeLeft -= Time.deltaTime;
        }
        else if ((playerScript.isSliding && slideTimeLeft < 0 && !cantStopSliding) || !playerScript.isGrounded)
        {
            playerScript.isSliding = false;
            slideTimeLeft = playerScript.slideTime;
        }
    }
}
