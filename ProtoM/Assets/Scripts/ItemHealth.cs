using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHealth : MonoBehaviour
{
    public int value;
    public GameObject gm;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindWithTag("MainCamera");
        gameManager = gm.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (gameManager.playerHealth < 28)
            {
                Destroy(gameObject);
                if ((gameManager.playerHealth + value) > 28)
                {
                    gameManager.playerHealth = 28;
                }
                else
                {
                    gameManager.playerHealth += value;
                }
            }
        }
    }
}
