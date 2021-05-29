using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public GameObject gm;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindWithTag("MainCamera");
        gameManager = gm.GetComponent<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            gameManager.playerHealth = 0;
        }
    }
}
