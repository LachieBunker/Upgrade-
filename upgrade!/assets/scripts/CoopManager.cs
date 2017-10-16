using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CoopManager : MonoBehaviour {

    public GameObject player1Object;
    PlayerController player1Script;
    public GameObject player1Spawn;
    public GameObject player2Object;
    PlayerController player2Script;
    public GameObject player2Spawn;


    public GameObject enemyObject;
    public GameObject[] enemySpawns;
    
    public float playerAbilityTimer;
    public float playerAbilityDelay = 40;

    public float enemySpawnTimer;//[]
    public float enemySpawnDelay = 5;
    public float enemySpawnDelayIncrement = 0.8f;
    public int enemySpawnSpeedUpCount = 5;

    MenuManager menuManager;

    public int player1Lives = 3;
    public int player2Lives = 3;

    public GameObject hudCanvas_PvP;
    public Sprite[] numSprites;
    public Slider player1HealthBar;
    public List<Image> player1AbilitySprites;
    public Image player1AbilityImage;
    public Image player1LivesImage;
    public Slider player2HealthBar;
    public List<Image> player2AbilitySprites;
    public Image player2AbilityImage;
    public Image player2LivesImage;

    public int enemiesKilled = 0;
    public float enemyUpgradeGoal = 5;
    float enemyUpgradeGoalIncrement = 1.5f;

    // Use this for initialization
    void Start()
    {

        menuManager = GameObject.FindWithTag("MenuManager").GetComponent<MenuManager>();
        player1Spawn = GameObject.FindWithTag("Player1Spawn");
        GameObject player1 = (GameObject)Instantiate(player1Object, player1Spawn.transform.position, player1Spawn.transform.rotation);
        player1Script = player1.GetComponent<PlayerController>();
        player1Script.playerOwner = AttackOwner.Player;
        player2Spawn = GameObject.FindWithTag("Player2Spawn");
        GameObject player2 = (GameObject)Instantiate(player2Object, player2Spawn.transform.position, player2Spawn.transform.rotation);
        player2Script = player2.GetComponent<PlayerController>();
        player2Script.playerOwner = AttackOwner.Player;

        enemySpawns = GameObject.FindGameObjectsWithTag("EnemySpawn");
        enemySpawnTimer = Time.time + enemySpawnDelay;
        //Instantiate player

        player1Lives = 3;
        player2Lives = 3;
        
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
            player1Script.GetAbility();
            player2Script.GetAbility();
        }
    }

    public void UpdateHUD()
    {
        player1HealthBar.maxValue = player1Script.currentMaxHealth;
        //Debug.Log(player1Script.currentHealth);
        player1HealthBar.value = player1Script.currentHealth;
        player1LivesImage.sprite = numSprites[player1Lives];
        if (player1Script.currentAbilityImages.Count > 0)
        {
            player1AbilityImage.gameObject.SetActive(true);
        }
        else
        {
            player1AbilityImage.gameObject.SetActive(false);
        }
        if (player1Script.abilityReady)
        {
            player1AbilityImage.color = Color.white;
        }
        else
        {
            player1AbilityImage.color = Color.gray;
        }
        player2HealthBar.maxValue = player2Script.currentMaxHealth;
        //Debug.Log(player2Script.currentHealth);
        player2HealthBar.value = player2Script.currentHealth;
        player2LivesImage.sprite = numSprites[player2Lives];
        if (player2Script.currentAbilityImages.Count > 0)
        {
            player2AbilityImage.gameObject.SetActive(true);
        }
        else
        {
            player2AbilityImage.gameObject.SetActive(false);
        }
        if (player2Script.abilityReady)
        {
            player2AbilityImage.color = Color.white;
        }
        else
        {
            player2AbilityImage.color = Color.gray;
        }
    }
    public void EnemyKilled()
    {
        enemiesKilled++;
        if (enemiesKilled >= enemyUpgradeGoal)
        {
            enemiesKilled = 0;
            enemyUpgradeGoal = enemyUpgradeGoal * enemyUpgradeGoalIncrement;
            if (player1Object.activeInHierarchy)
            {
                player1Script.Upgrade();
            }
            if (player2Object.activeInHierarchy)
            {
                player2Script.Upgrade();
            }
        }
    }

    public void SpawnEnemy()
    {
        int spawnNum = Random.Range(0, enemySpawns.Length);
        GameObject enemy = (GameObject)Instantiate(enemyObject, enemySpawns[spawnNum].transform.position, enemySpawns[spawnNum].transform.rotation);
        enemy.GetComponent<EnemyController>().ScaleStats(player1Script.currentUpgrades.Count + player2Script.currentUpgrades.Count);//*0.75f
    }

    public void PlayerDead(string currentPlayer, GameObject playerObject)
    {
        switch (currentPlayer)
        {
            case "Player1":
                player1Lives--;
                UpdateHUD();
                if (player1Lives > 0)
                {
                    playerObject.transform.position = player1Spawn.transform.position;
                    playerObject.GetComponent<PlayerController>().Respawn();
                    UpdateHUD();
                    playerObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                }
                else
                {
                    playerObject.SetActive(false);
                }
                break;
            case "Player2":
                player2Lives--;
                UpdateHUD();
                if (player2Lives > 0)
                {
                    playerObject.transform.position = player2Spawn.transform.position;
                    playerObject.GetComponent<PlayerController>().Respawn();
                    UpdateHUD();
                    playerObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                }
                else
                {
                    playerObject.SetActive(false);
                }
                break;
        }

        if (player1Lives <= 0 && player2Lives <= 0)
        {
            menuManager.GameLost();
        }
    }
}
