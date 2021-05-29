using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float enemyHealth = 28;
    public float power;
    public int lemonDamage = 1;

    AudioSource _audio;
    public AudioClip hitSound;

    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            _audio.PlayOneShot(hitSound);
            GameObject lemon = collision.gameObject;
            Lemon lemonScript = lemon.GetComponent<Lemon>();
            enemyHealth -= lemonScript.power * lemonDamage;
            if (lemonScript.power < enemyHealth)
            {
                Destroy(lemon);
            }
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
