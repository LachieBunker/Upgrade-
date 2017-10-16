using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

    public List<InputTypes> recentInputs = new List<InputTypes>();
    public List<UpgradesInterface> availableUpgrades = new List<UpgradesInterface>();
    public List<UpgradesInterface> currentUpgrades = new List<UpgradesInterface>();

    public UpgradesManager upgradesManager;
    GameController gameController;
    Rigidbody2D rB;
    Animator animator;
    public int directionFacing = 1;
    public float moveInputTimer;
    float moveInputDelay = 2f;
    public GameObject attackObject;
    public float attackDelay;
    public float abilityDelay;
    public bool abilityReady;
    bool abilityImageUpdated;
    public float invulTime;
    public float invulDelay = 0.5f;
    public string currentPlayer;
    public AttackOwner playerOwner;

    public GameObject upgradeImage;
    public GameObject recentUpgrade;
    public List<Sprite> currentAbilityImages;

    public PlayerInput moveAxis;
    public PlayerInput jumpButton;
    public PlayerInput attackButton;
    public PlayerInput abilityButton;

    //Idle
    //Move
    int baseMoveSpeed = 1;
    //Jump
    int baseNumJumps = 1;
    int baseJumpHeight = 4;
    int baseGravity = 1;
    //Attack
    int baseDamage = 5;
    int baseSize = 5;
    int baseKnockback = 100;
    int baseAttackSpeed = 2;
    //AttackCoolDown
    int baseAttackCoolDown = 2;
    float baseRange = 0.5f;//0.05f;
    [Range(1, 100)]
    int baseNumAttacks = 1;
    int baseAngle = 0;
    int baseAttackHealth = 1;
    //Ability

    //AbilityCoolDown
    int baseAbilityCoolDown = 5;

    int baseHealth = 20;

    //Idle
    //Move
    public int currentMoveSpeed;
    //Jump
    public int currentNumJumps;
    public int numJumpsAvailable;
    public int currentJumpHeight;
    public int currentGravity;
    //Attack
    [Range(1, 50)]
    public int currentDamage;
    [Range(1, 20)]
    public int currentSize;
    public int currentKnockback;
    public int currentAttackSpeed;
    //AttackCoolDown
    public float currentAttackCoolDown;
    public float currentRange;
    [Range(1, 50)]
    public int currentNumAttacks;
    public int currentAngle;
    public int currentAttackHealth;
    //Ability

    //AbilityCoolDown
    public int currentAbilityCoolDown;

    public int currentHealth;
    public int currentMaxHealth;

    // Use this for initialization
    void Start()
    {
        upgradesManager = GameObject.FindGameObjectWithTag("Upgrades").GetComponent<UpgradesManager>();
        //gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        

        rB = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();

        moveInputTimer = Time.time + moveInputDelay;

        ResetCurrentStats();

        numJumpsAvailable = currentNumJumps;

        for (int i = 0; i < UpgradesDictionary.instance.upgrades.Count; i++)
        {
            availableUpgrades.Add(UpgradesDictionary.instance.upgrades[i]);
        }

        DisplayAbilities();

        switch (GameModeManager.gameModeToLoad)
        {
            case GameModes.Single:
                GameObject.FindWithTag("SPManager").GetComponent<SinglePlayerManager>().UpdateHUD();
                break;
            case GameModes.Coop:
                GameObject.FindWithTag("CPManager").GetComponent<CoopManager>().UpdateHUD();
                break;
            case GameModes.PvP:
                GameObject.FindWithTag("PPManager").GetComponent<PvPManager>().UpdateHUD();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(GetPlayerKey(attackButton)))
        {
            if (attackDelay <= Time.time)
            {
                float attackWaitIncrement = (0.5f / currentAttackCoolDown);
                if (attackWaitIncrement < 0.1)
                {
                    attackWaitIncrement = 0.1f;
                }
                attackDelay = Time.time + attackWaitIncrement;
                //Debug.Log(attackWaitIncrement);
                recentInputs.Add(InputTypes.Attack);
                UpdateAnimations("Fire");
                Attack();
            }
            else
            {
                recentInputs.Add(InputTypes.AttackCoolDown);
            }
        }
        else if (Input.GetKeyDown(GetPlayerKey(abilityButton)))
        {
            if (abilityDelay <= Time.time)
            {
                float abilityWaitIncrement = currentAbilityCoolDown;// (0.5f / currentAbilityCoolDown);
                if (abilityWaitIncrement < 0.1)
                {
                    abilityWaitIncrement = 0.1f;
                }
                abilityDelay = Time.time + abilityWaitIncrement;
                //Debug.Log(abilityWaitIncrement);
                //recentInputs.Add(InputTypes.Attack);
                //UpdateAnimations("Fire");
                Ability();
            }
            else
            {
                recentInputs.Add(InputTypes.AttackCoolDown);//MaybeChange
            }
        }
        else if (Input.GetKeyDown(GetPlayerKey(jumpButton)))
        {
            recentInputs.Add(InputTypes.Jump);
            CheckIfOnGround();
            if (numJumpsAvailable > 0)
            {
                rB.AddForce(Vector2.up * currentJumpHeight, ForceMode2D.Impulse);
                numJumpsAvailable--;
            }
        }/*
        else if (Input.GetKeyDown(KeyCode.M))
        {
            recentInputs.Add(InputTypes.Move);
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            GetAbility();
        }*/
        else if (Input.GetAxis(GetPlayerAxis(moveAxis)) < 0)
        {
            directionFacing = -1;
            UpdateAnimations("Walk");
            transform.position += Vector3.left * currentMoveSpeed * Time.deltaTime;
            if (moveInputTimer <= Time.time)
            {
                recentInputs.Add(InputTypes.Move);
                moveInputTimer = Time.time + moveInputDelay;
            }
        }
        else if (Input.GetAxis(GetPlayerAxis(moveAxis)) > 0)
        {
            directionFacing = 1;
            UpdateAnimations("Walk");
            transform.position += Vector3.right * currentMoveSpeed * Time.deltaTime;
            if (moveInputTimer <= Time.time)
            {
                recentInputs.Add(InputTypes.Move);
                moveInputTimer = Time.time + moveInputDelay;
            }
        }
        else
        {
            UpdateAnimations("Idle");
        }

        if(abilityDelay <= Time.time && abilityImageUpdated == false)
        {
        	abilityReady = true;
        	abilityImageUpdated = true;
			switch (GameModeManager.gameModeToLoad)
            {
                case GameModes.Single:
                    GameObject.FindWithTag("SPManager").GetComponent<SinglePlayerManager>().UpdateHUD();
                    break;
                case GameModes.Coop:
                    GameObject.FindWithTag("CPManager").GetComponent<CoopManager>().UpdateHUD();
                    break;
                case GameModes.PvP:
                    GameObject.FindWithTag("PPManager").GetComponent<PvPManager>().UpdateHUD();
                    break;
            }
        }
    }

    public void Upgrade()
    {
        InputTypes firstPopInput;
        InputTypes secondPopInput;
        upgradesManager.GetPopularInputs(recentInputs, out firstPopInput, out secondPopInput);
        //Debug.Log("" + firstPopInput + secondPopInput);
        UpgradesInterface newUpgrade = upgradesManager.GetUpgrade(firstPopInput, currentUpgrades, availableUpgrades);
        availableUpgrades.Remove(newUpgrade);
        recentInputs.Clear();
        if (newUpgrade != null)
        {
            currentUpgrades.Add(newUpgrade);
            DisplayUpgradeNotification(newUpgrade.upgradeImage);
        }
        else
        {
            return;
        }

        
        //Debug.Log(currentUpgrades[0].inputType1);

        ResetCurrentStats();
        ApplyUpgrades();

        switch (GameModeManager.gameModeToLoad)
        {
            case GameModes.Single:
                GameObject.FindWithTag("SPManager").GetComponent<SinglePlayerManager>().UpdateHUD();
                break;
            case GameModes.Coop:
                GameObject.FindWithTag("CPManager").GetComponent<CoopManager>().UpdateHUD();
                break;
            case GameModes.PvP:
                GameObject.FindWithTag("PPManager").GetComponent<PvPManager>().UpdateHUD();
                break;
        }

    }

    public void ResetCurrentStats()
    {
        //Idle
        //Move
        currentMoveSpeed = baseMoveSpeed;
        //Jump
        currentNumJumps = baseNumJumps;
        currentJumpHeight = baseJumpHeight;
        currentGravity = baseGravity;
        //Attack;
        currentDamage = baseDamage;
        currentSize = baseSize;
        currentKnockback = baseKnockback;
        currentAttackSpeed = baseAttackSpeed;
        //AttackSpeed
        currentAttackCoolDown = baseAttackCoolDown;
        currentRange = baseRange;
        currentNumAttacks = baseNumAttacks;
        currentAngle = baseAngle;
        currentAttackHealth = baseAttackHealth;
        //Ability

        //AbilityCoolDown
        currentAbilityCoolDown = baseAbilityCoolDown;

        //Other
        currentMaxHealth = baseHealth + (int)(baseHealth * currentUpgrades.Count * 0.75f);
        currentHealth = currentMaxHealth;
    }

    public void ApplyUpgrades()
    {
        for (int i = 0; i < currentUpgrades.Count; i++)
        {
            switch (currentUpgrades[i].inputType1)
            {
                case InputTypes.Idle:

                    break;
                case InputTypes.Move:
                    currentUpgrades[i].GetMovementProperties(currentMoveSpeed, currentMaxHealth, out currentMoveSpeed, out currentMaxHealth);
                    break;
                case InputTypes.Jump:
                    currentUpgrades[i].GetJumpProperties(currentNumJumps, currentJumpHeight, currentGravity, out currentNumJumps, out currentJumpHeight, out currentGravity);
                    break;
                case InputTypes.Attack:
                    currentUpgrades[i].GetAttackObjectProperties(currentDamage, currentSize, currentKnockback, currentAttackSpeed, out currentDamage, out currentSize, out currentKnockback, out currentAttackSpeed);
                    break;
                case InputTypes.AttackCoolDown:
                    currentUpgrades[i].GetAttackCoolDownProperties(currentAttackCoolDown, currentRange, currentNumAttacks, currentAngle, currentAttackHealth, out currentAttackCoolDown, out currentRange, out currentNumAttacks, out currentAngle, out currentAttackHealth);
                    break;
                case InputTypes.Ability:

                    break;
                case InputTypes.AbilityCoolDown:
                    currentUpgrades[i].GetAbilityCoolDownProperties(currentAbilityCoolDown, out currentAbilityCoolDown);
                    break;
            }
            switch (currentUpgrades[i].inputType2)
            {
                case InputTypes.Idle:

                    break;
                case InputTypes.Move:
                    currentUpgrades[i].GetMovementProperties(currentMoveSpeed, currentMaxHealth, out currentMoveSpeed, out currentMaxHealth);
                    break;
                case InputTypes.Jump:
                    currentUpgrades[i].GetJumpProperties(currentNumJumps, currentJumpHeight, currentGravity, out currentNumJumps, out currentJumpHeight, out currentGravity);
                    break;
                case InputTypes.Attack:
                    currentUpgrades[i].GetAttackObjectProperties(currentDamage, currentSize, currentKnockback, currentAttackSpeed, out currentDamage, out currentSize, out currentKnockback, out currentAttackSpeed);
                    break;
                case InputTypes.AttackCoolDown:
                    currentUpgrades[i].GetAttackCoolDownProperties(currentAttackCoolDown, currentRange, currentNumAttacks, currentAngle, currentAttackHealth, out currentAttackCoolDown, out currentRange, out currentNumAttacks, out currentAngle, out currentAttackHealth);
                    break;
                case InputTypes.Ability:

                    break;
                case InputTypes.AbilityCoolDown:
                    currentUpgrades[i].GetAbilityCoolDownProperties(currentAbilityCoolDown, out currentAbilityCoolDown);
                    break;
            }
        }
        currentHealth = currentMaxHealth;
        //Update UI
    }

    public void DisplayAbilities()
    {
        switch (GameModeManager.gameModeToLoad)
        {
            case GameModes.Single:
                SinglePlayerManager manager = GameObject.FindWithTag("SPManager").GetComponent<SinglePlayerManager>();
                for (int i = 0; i < manager.playerAbilitySprites.Count; i++)
                {
                    if (i < currentAbilityImages.Count)
                    {
                        manager.playerAbilitySprites[i].GetComponent<Image>().sprite = currentAbilityImages[i];
                        manager.playerAbilitySprites[i].enabled = true;
                    }
                    else
                    {
                        manager.playerAbilitySprites[i].enabled = false;
                    }
                }
                manager.UpdateHUD();
                break;
            case GameModes.Coop:
                CoopManager Cmanager = GameObject.FindWithTag("CPManager").GetComponent<CoopManager>();
                if (currentPlayer == "Player1")
                {
                    for (int i = 0; i < Cmanager.player1AbilitySprites.Count; i++)
                    {
                        if (i < currentAbilityImages.Count)
                        {
                            Cmanager.player1AbilitySprites[i].GetComponent<Image>().sprite = currentAbilityImages[i];
                            Cmanager.player1AbilitySprites[i].enabled = true;
                        }
                        else
                        {
                            Cmanager.player1AbilitySprites[i].enabled = false;
                        }
                    }
                }
                else if (currentPlayer == "Player2")
                {
                    for (int i = 0; i < Cmanager.player2AbilitySprites.Count; i++)
                    {
                        if (i < currentAbilityImages.Count)
                        {
                            Cmanager.player2AbilitySprites[i].GetComponent<Image>().sprite = currentAbilityImages[i];
                            Cmanager.player2AbilitySprites[i].enabled = true;
                        }
                        else
                        {
                            Cmanager.player2AbilitySprites[i].enabled = false;
                        }
                    }
                }
                Cmanager.UpdateHUD();
                break;
            case GameModes.PvP:
                PvPManager Pmanager = GameObject.FindWithTag("PPManager").GetComponent<PvPManager>();
                if (currentPlayer == "Player1")
                {
                    for (int i = 0; i < Pmanager.player1AbilitySprites.Count; i++)
                    {
                        if (i < currentAbilityImages.Count)
                        {
                            Pmanager.player1AbilitySprites[i].GetComponent<Image>().sprite = currentAbilityImages[i];
                            Pmanager.player1AbilitySprites[i].enabled = true;
                        }
                        else
                        {
                            Pmanager.player1AbilitySprites[i].enabled = false;
                        }
                    }
                }
                else if (currentPlayer == "Player2")
                {
                    for (int i = 0; i < Pmanager.player2AbilitySprites.Count; i++)
                    {
                        if (i < currentAbilityImages.Count)
                        {
                            Pmanager.player2AbilitySprites[i].GetComponent<Image>().sprite = currentAbilityImages[i];
                            Pmanager.player2AbilitySprites[i].enabled = true;
                        }
                        else
                        {
                            Pmanager.player2AbilitySprites[i].enabled = false;
                        }
                    }
                }
                Pmanager.UpdateHUD();
                break;
        }
        
    }

    public void DisplayUpgradeNotification(Sprite upgradeSprite)
    {
        recentUpgrade.GetComponent<SpriteRenderer>().sprite = upgradeSprite;
        upgradeImage.SetActive(true);
        StartCoroutine(HideUpgradeNotification());
    }

    public IEnumerator HideUpgradeNotification()
    {
        yield return new WaitForSeconds(2);
        upgradeImage.SetActive(false);
    }

    public void GetAbility()
    {
        InputTypes firstPopInput;
        InputTypes secondPopInput;
        upgradesManager.GetPopularInputs(recentInputs, out firstPopInput, out secondPopInput);
        UpgradesInterface newAbility = upgradesManager.GetAbility(firstPopInput, currentUpgrades, availableUpgrades);
        availableUpgrades.Remove(newAbility);
        recentInputs.Clear();
        if (newAbility != null)
        {
            currentUpgrades.Add(newAbility);
            DisplayUpgradeNotification(newAbility.upgradeImage);
            currentAbilityImages.Add(newAbility.upgradeImage);
        }
        else
        {
            return;
        }
        DisplayAbilities();
    }

    public void CheckIfOnGround()
    {
        RaycastHit2D hit;
        if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.2f), Vector2.down, 0.1f))
        {
            hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.2f), Vector2.down, 0.1f);
            //Debug.DrawLine(new Vector2(transform.position.x, transform.position.y - 0.2f), hit.transform.position, Color.magenta);
            //Debug.Log(hit.transform.tag);
            if (hit.transform.tag == "Ground" || hit.transform.tag == "Platform")
            {
                numJumpsAvailable = currentNumJumps;
                //Debug.Log(numJumpsAvailable);
            }
        }
        else
        {
            //Debug.Log("HitNothing");
        }
        
    }

    public void Attack()
    {
        if (currentNumAttacks < 1)
        {
            currentNumAttacks = 1;
        }
        if (currentDamage < 1)
        {
            currentDamage = 1;
        }
        if (currentSize < 2)
        {
            currentSize = 2;
        }
        if (currentSize > 20)
        {
            currentSize = 20;
        }
        if (currentKnockback > 200)
        {
            currentKnockback = 200;
        }
        //Mathf.Clamp (currentNumAttacks, 1, 50);
        for (int i = 0; i < currentNumAttacks; i++)
        {
            GameObject attack = (GameObject)Instantiate(attackObject, new Vector2(transform.position.x + (0.1f * directionFacing), transform.position.y + 0.045f), Quaternion.Euler(0, 0, (Random.Range(90-currentAngle/2, 90+currentAngle/2))));
            AttackObjectScript attackScript = attack.GetComponent<AttackObjectScript>();
			Mathf.Clamp (currentDamage, 1, 50);
			Mathf.Clamp (currentSize, 1, 20);
            int tempKnockBack;
            if (GameModeManager.gameModeToLoad == GameModes.PvP)
            {
                tempKnockBack = currentKnockback / 4;
            }
            else
            {
                tempKnockBack = currentKnockback;
            }
            attackScript.SetAttackValues(playerOwner, currentDamage, currentSize, tempKnockBack, currentRange, (currentAttackSpeed * directionFacing), currentAttackHealth);
            attack.transform.localScale = new Vector2(attack.transform.localScale.x * currentSize * 0.2f, attack.transform.localScale.y * (currentSize/5 * 0.5f));
            //attack.GetComponent<BoxCollider2D>().size = attack.GetComponent<BoxCollider2D>().size * currentSize * 0.2f;
            //attack.GetComponent<Rigidbody2D>().AddForce(new Vector2(directionFacing, 0));

        }
    }

    public void Ability()
    {
        for (int i = 0; i < currentUpgrades.Count; i++)
        {
            if (currentUpgrades[i].inputType1 == InputTypes.Ability)
            {
                currentUpgrades[i].UseAbility(this, gameObject);
            }
		}
    	abilityReady = false;
    	abilityImageUpdated = false;
		switch (GameModeManager.gameModeToLoad)
            {
                case GameModes.Single:
                    GameObject.FindWithTag("SPManager").GetComponent<SinglePlayerManager>().UpdateHUD();
                    break;
                case GameModes.Coop:
                    GameObject.FindWithTag("CPManager").GetComponent<CoopManager>().UpdateHUD();
                    break;
                case GameModes.PvP:
                    GameObject.FindWithTag("PPManager").GetComponent<PvPManager>().UpdateHUD();
                    break;
            }
    }

    public void Damaged(int amountDamaged)
    {
        if (invulTime <= Time.time)
        {
            invulTime = Time.time + invulDelay;
            currentHealth -= amountDamaged;
            switch (GameModeManager.gameModeToLoad)
            {
                case GameModes.Single:
                    GameObject.FindWithTag("SPManager").GetComponent<SinglePlayerManager>().UpdateHUD();
                    break;
                case GameModes.Coop:
                    GameObject.FindWithTag("CPManager").GetComponent<CoopManager>().UpdateHUD();
                    break;
                case GameModes.PvP:
                    GameObject.FindWithTag("PPManager").GetComponent<PvPManager>().UpdateHUD();
                    break;
            }
            if (currentHealth <= 0)
            {
                Dead();
            }
        }
        
    }

    public void Dead()
    {
        switch (GameModeManager.gameModeToLoad)
        {
            case GameModes.Single:
                GameObject.FindWithTag("SPManager").GetComponent<SinglePlayerManager>().PlayerDead();
                break;
            case GameModes.Coop:
                GameObject.FindWithTag("CPManager").GetComponent<CoopManager>().PlayerDead(currentPlayer, gameObject);
                break;
            case GameModes.PvP:
                GameObject.FindWithTag("PPManager").GetComponent<PvPManager>().PlayerDead(currentPlayer, gameObject);
                break;
        }
    }

    public void Respawn()
    {
        currentHealth = currentMaxHealth;
    }

    public string GetPlayerAxis(PlayerInput inputAxis)
    {
        switch (inputAxis)
        {
            case PlayerInput.Move:
                return "Horizontal";
            case PlayerInput.AltMove:
                return "AltHorizontal";
        }
        return null;
    }

    public KeyCode GetPlayerKey(PlayerInput inputKey)
    {
        switch (inputKey)
        {
            case PlayerInput.Jump:
                return KeyCode.UpArrow;
            case PlayerInput.Attack:
                return KeyCode.LeftControl;
            case PlayerInput.Ability:
                return KeyCode.LeftShift;
            case PlayerInput.AltJump:
                return KeyCode.W;
            case PlayerInput.AltAttack:
                return KeyCode.N;
            case PlayerInput.AltAbility:
                return KeyCode.M;
        }

        return KeyCode.Escape;
    }

    public void UpdateAnimations(string animationState)
    {
        switch (animationState)
        {
            case "Idle":
                animator.SetBool("Walking", false);
                animator.SetBool("Firing", false);
                break;
            case "Walk":
                animator.SetBool("Walking", true);
                animator.SetBool("Firing", false);
                gameObject.transform.localScale = new Vector3(directionFacing, 1);
                upgradeImage.transform.localScale = new Vector2(0.75f * directionFacing, 0.75f);
                break;
            case "Fire":
                animator.SetBool("Walking", false);
                animator.SetBool("Firing", true);
                gameObject.transform.localScale = new Vector3(directionFacing, 1);
                upgradeImage.transform.localScale = new Vector2(0.75f * directionFacing, 0.75f);
                break;
        }
    }

}
