using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class SinglePlayerManager : MonoBehaviour {

    public GameObject playerObject;
    PlayerController playerScript;
    public GameObject playerSpawn;
    public GameObject enemyObject;

    public float playerAbilityTimer;
    public float playerAbilityDelay = 40;

    public float enemySpawnTimer;//[]
    public float enemySpawnDelay = 5;
    public float enemySpawnDelayIncrement = 0.8f;
    public int enemySpawnSpeedUpCount = 5;
    public GameObject[] enemySpawns;//[]

    MenuManager menuManager;
    
    public GameObject hudCanvas_Single;
    public Slider playerHealthBar;
    public List<Image> playerAbilitySprites;
    public Image playerAbilityImage;

    public int enemiesKilled = 0;
    public float enemyUpgradeGoal = 5;
    float enemyUpgradeGoalIncrement = 1.5f;

    // Use this for initialization
    void Start()
    {

        menuManager = GameObject.FindWithTag("MenuManager").GetComponent<MenuManager>();
        playerSpawn = GameObject.FindWithTag("PlayerSpawn");
        enemySpawns = GameObject.FindGameObjectsWithTag("EnemySpawn");
        //hudCanvas_Single = GameObject.FindWithTag("HUDSingle");
        //playerHealthBar = hudCanvas_Single.GetComponentInChildren<Slider>();
        enemySpawnTimer = Time.time + enemySpawnDelay;
        GameObject player = (GameObject)Instantiate(playerObject, playerSpawn.transform.position, playerSpawn.transform.rotation);
        playerScript = player.GetComponent<PlayerController>();
        //Instantiate player

        playerAbilityTimer = Time.time + 20;
        UpdateHUD();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemySpawnTimer <= Time.time)
        {
            if (enemySpawnDelay < 0.5f)
            {
                enemySpawnDelay = 0.5f;
            }
            enemySpawnSpeedUpCount--;
            if (enemySpawnSpeedUpCount <= 0)
            {
                enemySpawnSpeedUpCount = 5;
                enemySpawnDelay = enemySpawnDelay * enemySpawnDelayIncrement;
            }
            enemySpawnTimer = Time.time + enemySpawnDelay;
            SpawnEnemy();
        }
        if (playerAbilityTimer <= Time.time)
        {
            playerAbilityTimer = Time.time + playerAbilityDelay;
            playerScript.GetAbility();
        }
    }

    public void UpdateHUD()
    {
        playerHealthBar.maxValue = playerScript.currentMaxHealth;
        //Debug.Log("" + Mathf.InverseLerp(0, playerScript.currentMaxHealth, playerScript.currentHealth));
        //Debug.Log(playerScript.currentHealth);
        playerHealthBar.value = playerScript.currentHealth;// Mathf.InverseLerp(0, playerScript.currentMaxHealth, playerScript.currentHealth);
        if(playerScript.currentAbilityImages.Count > 0)
        {
        	playerAbilityImage.gameObject.SetActive(true);
        }
        else
        {
        	playerAbilityImage.gameObject.SetActive(false);
        }
        if(playerScript.abilityReady)
        {
        	playerAbilityImage.color = Color.white;
        }
        else
        {
        	playerAbilityImage.color = Color.gray;
        }
    }

    public void EnemyKilled()
    {
        enemiesKilled++;
        if (enemiesKilled >= enemyUpgradeGoal)
        {
            enemiesKilled = 0;
            enemyUpgradeGoal = enemyUpgradeGoal * enemyUpgradeGoalIncrement;
            playerScript.Upgrade();
        }
    }

    public void PlayerDead()
    {
        menuManager.GameLost();
    }

    public void SpawnEnemy()
    {
        int spawnNum = Random.Range(0, enemySpawns.Length);
        GameObject enemy = (GameObject)Instantiate(enemyObject, enemySpawns[spawnNum].transform.position, enemySpawns[spawnNum].transform.rotation);
        enemy.GetComponent<EnemyController>().ScaleStats(playerScript.currentUpgrades.Count);//*0.75f
    }

    /*public void GameLost()
    {
        Time.timeScale = 0.0f;
        gameLostCanvas.SetActive(true);
    }*/
}
