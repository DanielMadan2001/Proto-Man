using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public GameObject player;
    public PlayerScriptBig playerScriptBig;

    public Collider2D layerTop;
    public bool layerTopCollision;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindWithTag("Player");
        playerScriptBig = player.GetComponent<PlayerScriptBig>();
        if (playerScriptBig.isOnLadder)
        {
            layerTopCollision = false;
        }
        else
        {
            layerTopCollision = true;
        }
        layerTop.GetComponent<BoxCollider2D>().enabled = layerTopCollision;
    }
}
