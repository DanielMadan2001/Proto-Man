using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public static bool isPaused = false;

    public GameObject gm;
    public GameManager gameManager;

    public GameObject pauseMenu;
    public Text currentHealthText;
    public Text livesText;
    public Text eTankText;
    public Text mTankText;
    public Text energyBalancerText;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindWithTag("MainCamera");
        gameManager = gm.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1f;
                isPaused = false;
            }
            else
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0f;
                isPaused = true;
            }
        }

        currentHealthText.text = "Current Health: " + gameManager.playerHealth;
        livesText.text = "Lives: " + gameManager.lives;
        eTankText.text = "E Tanks: " + gameManager.energyTankCount;
        mTankText.text = "M Tanks: " + gameManager.mysteryTankCount;
        if (gameManager.energyBalancer)
        {
            energyBalancerText.text = "Energy Balancer Found";
        }
        else
        {
            energyBalancerText.text = "Energy Balancer Missing";
        }
    }
}
