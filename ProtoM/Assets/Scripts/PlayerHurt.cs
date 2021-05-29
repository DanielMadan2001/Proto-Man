using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurt : MonoBehaviour
{
    public Animator _animator;
    Transform _transform;
    Rigidbody2D _rigidbody;
    SpriteRenderer _sprite;

    public GameManager gameManager;
    public PlayerScriptBig playerScript;

    public float stunTime;
    private float stunTimeLeft;
    public float invulPeriod;
    private float invulPeriodLeft;
    public bool isStunned;
    public bool isInvul;
    public bool triggerDamage;


    // Start is called before the first frame update
    void Start()
    {
        stunTimeLeft = stunTime;
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();

        gameManager = GameObject.FindWithTag("MainCamera").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Stun();
        Invul();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            GameObject enemy = other.gameObject;
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (triggerDamage)
            {
                gameManager.playerHealth -= enemyScript.power;
                triggerDamage = false;
            }
            isStunned = true;
        }
    }

    void Stun()
    {
        if (isStunned)
        {
            _animator.SetBool("PlayerHurt1", true);
            stunTimeLeft -= Time.deltaTime;
            playerScript.canMove = false;
            _rigidbody.velocity = new Vector2(0,0);
            Physics2D.IgnoreLayerCollision(8, 10, true);
        }

        if (stunTimeLeft < 0)
        {
            _animator.SetBool("PlayerHurt1", false);
            playerScript.canMove = true;
            isStunned = false;
            stunTimeLeft = stunTime;
            invulPeriodLeft = invulPeriod;
            isInvul = true;
        }
    }

    void Invul()
    {
        if (isInvul)
        {
            invulPeriodLeft -= Time.deltaTime;
        }
        
        if (invulPeriodLeft <= 0)
        {
            Physics2D.IgnoreLayerCollision(8, 10, false);
            isInvul = false;
            triggerDamage = true;
        }
    }
}
