using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public enum currentWeapon { PROTO_BUSTER, PROTO_SHIELD, MOTOR_SLIDE, HYDRO_HOSE, CLOCKWORK_FREEZE, CIPHER_SHOT, COMET_DART, BLITZ_MISSILE, WIZARD_WALL, PAINT_FLOOD }

public class GameManager : MonoBehaviour
{
    public float playerHealth = 28;
    public int lives = 3;
    public currentWeapon currentWeapon;
    public int energyTankCount;
    public int mysteryTankCount;
    public bool energyBalancer;

    public Transform checkPoint;
    public GameObject protoMan;
    public PlayerScriptBig playerScript;
    public GameObject death;
    public GameObject spawnPoint;

    public Text TestHealth;
    public Text TestLives;

    // Start is called before the first frame update
    void Start()
    {
        protoMan = GameObject.FindGameObjectWithTag("Player");
        playerScript = protoMan.GetComponent<PlayerScriptBig>();

    }

    // Update is called once per frame
    void Update()
    {
        GUI();
        PauseMenu();

        if (playerHealth <= 0)
        {
            Instantiate(death, protoMan.GetComponent<Transform>().position, Quaternion.identity);
            protoMan.GetComponent<Transform>().position = spawnPoint.GetComponent<Transform>().position;
            playerHealth = 28;
            lives -= 1;
            // StartCoroutine(KillPlayer());
        }
    }

    void GUI()
    {
        TestHealth.text = "" + playerHealth;
        TestLives.text = "" + lives;
    }

    void PauseMenu()
    {
        
    }

    IEnumerator KillPlayer()
    {
        int test = 0;
        Debug.Log("Phase 1: Moment of impact");
        // Time.timeScale = 0f;
        yield return new WaitForSeconds(0.1f);

        Debug.Log("Phase 2: Resume");
        Time.timeScale = 1f;

        Debug.Log("Phase 3: Blood");
        // protoMan.SetActive(false);
        if (test == 0)
        {
            Instantiate(death, protoMan.GetComponent<Transform>().position, Quaternion.identity);
            lives -= 1;
            test += 1;
        }
        // yield return new WaitForSeconds(2.0f);

        Debug.Log("Phase 4: Respawn");
        Respawn();
        yield return new WaitForSeconds(0.0f);
    }

    void Respawn()
    {
        playerHealth = 28;
        protoMan.GetComponent<Transform>().position = spawnPoint.GetComponent<Transform>().position;
    }


}
