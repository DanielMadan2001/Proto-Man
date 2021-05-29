using UnityEngine;
using System.Collections;

public class Lemon : MonoBehaviour
{

    Transform _transform;
    Rigidbody2D _rigidbody;

    public float bulletForce;
    public float power;
    private float direction = 1;
    private float bulletLife = 4;
    public string damageType = "lemon";

    // Use this for initialization
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        _rigidbody.velocity = new Vector2(bulletForce, 0);
        bulletLife -= Time.deltaTime;
        if (bulletLife <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Flip(float x)
    {
        if ((x > 0 && bulletForce < 0) || (x < 0 && bulletForce > 0))
        {
            bulletForce *= -1;
        }

    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}