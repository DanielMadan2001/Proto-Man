using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public PlayerScript playerScript;

    Animator _animator;
    Rigidbody2D _rigidbody;
    Transform _transform;

    public Lemon lemonScript;
    public Lemon lemonScript2;
    public Lemon lemonScript3;
    public GameObject lemon;
    public GameObject lemon2;
    public GameObject lemon3;

    private float shootTimeLeft;
    private float shootButtonHeldDown;
    int bulletLevel = 1;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();

        shootTimeLeft = playerScript.shootTime;
    }

    void Update()
    {
        ShootCheck();
        Shooting();
        ChargeShot();
    }

    void ShootCheck()
    {
        if (Input.GetButtonDown("Fire1") && GameObject.FindGameObjectsWithTag("Bullet").Length < 2)
        {
            shootTimeLeft = playerScript.shootTime;
            playerScript.isShooting = true;
            Bullet();
        }
        else if (Input.GetButtonDown("Fire1") && GameObject.FindGameObjectsWithTag("Bullet").Length < 3)
        {
            playerScript.isShooting = true;
            Bullet();
        }
        else if (Input.GetButton("Fire2") && GameObject.FindGameObjectsWithTag("Bullet").Length < 3)
        {
            playerScript.isShooting = true;
            Bullet();
        }

        lemonScript.Flip(playerScript.direction);
        lemonScript2.Flip(playerScript.direction);
        lemonScript3.Flip(playerScript.direction);

        if ((playerScript.direction < 0 && lemon3.transform.localScale.x > 0) || (playerScript.direction > 0 && lemon3.transform.localScale.x < 0))
        {
            Vector2 theScale = new Vector2();
            theScale = lemon3.transform.localScale;
            theScale.x *= -1;
            lemon.transform.localScale = theScale;
            lemon2.transform.localScale = theScale;
            lemon3.transform.localScale = theScale;
        }

        _animator.SetBool("isShooting", playerScript.isShooting);
    }

    void Bullet()
    {
        float bx = _transform.position.x + 1.5f;
        float by = _transform.position.y - 0.3f;

        if (playerScript.isSliding)
        {
            bx += (1f * playerScript.direction);
            by += 0.3f;
        }

        if (playerScript.direction < 0)
        {
            bx -= 3f;
        }

        if (bulletLevel == 1)
        {
            GameObject b = Instantiate(lemon, new Vector3(bx, by, -1), Quaternion.identity) as GameObject;
        }
        else if (bulletLevel == 2)
        {
            GameObject b = Instantiate(lemon2, new Vector3(bx, by, -1), Quaternion.identity) as GameObject;
        }
        else
        {
            GameObject b = Instantiate(lemon3, new Vector3(bx, by, -1), Quaternion.identity) as GameObject;
        }
    }

    void Shooting()
    {
        if (playerScript.isShooting && playerScript.isGrounded && shootTimeLeft <= playerScript.shootTime && Input.GetAxisRaw("Horizontal") == 0 && !playerScript.isSliding && shootTimeLeft > 0)
        {
            playerScript.isIdleShooting = true;
            shootTimeLeft -= Time.deltaTime;

            if (playerScript.isShooting && playerScript.isGrounded && Input.GetAxisRaw("Horizontal") == 0 && shootTimeLeft < 0)
            {
                playerScript.isShooting = false;
                playerScript.isIdleShooting = false;
                shootTimeLeft = playerScript.shootTime;
            }
        }
        else if (playerScript.isShooting && shootTimeLeft <= playerScript.shootTime && shootTimeLeft > 0)
        {
            shootTimeLeft -= Time.deltaTime;

            if (playerScript.isShooting && shootTimeLeft < 0)
            {
                playerScript.isShooting = false;
                playerScript.isIdleShooting = false;
                shootTimeLeft = playerScript.shootTime;
            }
        }
    }

    void ChargeShot()
    {
        if (Input.GetButton("Fire1"))
        {
            shootButtonHeldDown += Time.deltaTime;
        }
        else if (Input.GetButtonUp("Fire1") && shootButtonHeldDown > 1.5)
        {
            Bullet();
            shootButtonHeldDown = 0;
            playerScript.isShooting = true;
        }

        if (shootButtonHeldDown > 1.5 && shootButtonHeldDown < 3)
        {
            bulletLevel = 2;
        }
        else if (shootButtonHeldDown > 2)
        {
            bulletLevel = 3;
        }
        else
        {
            bulletLevel = 1;
        }
    }
}
