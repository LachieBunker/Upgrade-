using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    Animator animator;

    public int directionFacing = 1;
    public int health = 10;
    public int patience = 5;
    int speed = 1;
    public GameObject attackObject;
    public float attackDelay;
    bool inRange;
    public bool stuck;

    int damage = 2;
    int attackSize = 5;
    int attackKnockBack = 50;
    int attackSpeed = 2;
    int attackCoolDown = 2;
    float attackRange = 0.1f;
    int numAttacks = 1;
    int attackAngle = 0;
    int attackHealth = 1;
    float invulTimer;
    float invulDelayAmount = 0.1f;
    float invulDelay = 0;

    // Use this for initialization
    void Start ()
    {
        animator = gameObject.GetComponent<Animator>();
        invulDelay = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!inRange && !stuck)
        {
            UpdateAnimations("Walk");
            transform.position += Vector3.right * directionFacing * speed * Time.deltaTime;
        }
        else
        {
            UpdateAnimations("Idle");
        }

        //inRange = false;
        RaycastHit2D hit;
        if (Physics2D.Raycast(new Vector2(transform.position.x + (0.1f * directionFacing), transform.position.y), Vector2.right * directionFacing))
        {
            hit = Physics2D.Raycast(new Vector2(transform.position.x + (0.1f * directionFacing), transform.position.y), Vector2.right * directionFacing);
            //Debug.DrawLine(gameObject.transform.position, hit.transform.position,Color.magenta);
            switch (hit.transform.tag)
            {
                case "Boundary":
                    if (Vector3.Distance(hit.transform.position, transform.position) <= 0.8f)
                    {
                        patience--;
                        if (patience <= 0)
                        {
                            patience = 5;
                            switch (GameModeManager.gameModeToLoad)
                            {
                                case GameModes.Single:
                                    SinglePlayerManager manager = GameObject.FindWithTag("SPManager").GetComponent<SinglePlayerManager>();
                                    int spawnNum = Random.Range(0, manager.enemySpawns.Length);
                                    //Debug.Log("New Enemy Spawn" + spawnNum);
                                    transform.position = manager.enemySpawns[spawnNum].transform.position;
                                    break;
                                case GameModes.Coop:
                                    CoopManager Cmanager = GameObject.FindWithTag("CPManager").GetComponent<CoopManager>();
                                    int CspawnNum = Random.Range(0, Cmanager.enemySpawns.Length);
                                    //Debug.Log("New Enemy Spawn" + CspawnNum);
                                    transform.position = Cmanager.enemySpawns[CspawnNum].transform.position;
                                    break;
                                case GameModes.PvP:

                                    break;
                            }
                            //transform.position = GameObject.FindWithTag("EnemySpawn").transform.position;
                        }
                        else
                        {
                            directionFacing *= -1;
                        }
                    }
                    else
                    {
                        inRange = false;
                    }
                    return;
                case "Player":
                case "Player1":
                case "Player2":
                    patience = 5;
                    if (Vector3.Distance(hit.transform.position, transform.position) <= 0.35f)
                    {
                        inRange = true;
                        if (attackDelay <= Time.time)
                        {
                            attackDelay = Time.time + (1f / (float)attackCoolDown);
                            UpdateAnimations("Fire");
                            Attack();
                        }
                    }
                    else
                    {
                        inRange = false;
                    }
                    return;
            }
            inRange = false;
        }
        if (Physics2D.Raycast(new Vector2(transform.position.x + (0.1f * -directionFacing), transform.position.y), Vector2.right * -directionFacing, 0.5f))
        {
            hit = Physics2D.Raycast(new Vector2(transform.position.x + (0.1f * -directionFacing), transform.position.y), Vector2.right * -directionFacing, 0.5f);
            if (hit.transform.tag == "Player" || hit.transform.tag == "Player1" || hit.transform.tag == "Player2")
            {
                directionFacing *= -1;
            }
        }


    }

    public void ScaleStats(float scaleAmount)
    {
        if (scaleAmount > 0)
        {
			health = (int)(health + (health * scaleAmount * 0.25));//0.5//0.375
            speed = (int)(speed + (speed * scaleAmount * 0.075));//0.1
			damage = (int)(damage + scaleAmount);//(damage * scaleAmount * 0.25));
            attackKnockBack = (int)(attackKnockBack + (attackKnockBack * scaleAmount * 0.075));
			attackCoolDown = (int)(attackCoolDown + (attackCoolDown * scaleAmount * 0.25));
        }
        if (scaleAmount >= 5)
        {
            invulDelay = invulDelayAmount + (invulDelayAmount * (scaleAmount/10));
        }
        //Debug.Log("health" + health + "speed" + speed + "damage" + damage + "KnockBack" + attackKnockBack + " CoolDown" + attackCoolDown + "Invul" + invulDelay);
    }

    public void Attack()
    {
        for (int i = 0; i < numAttacks; i++)
        {
            GameObject attack = (GameObject)Instantiate(attackObject, new Vector2(transform.position.x + (0.1f * directionFacing), transform.position.y + 0.01f), Quaternion.Euler(0, 0, (Random.Range(90 - attackAngle / 2, 90 + attackAngle / 2))));
            AttackObjectScript attackScript = attack.GetComponent<AttackObjectScript>();
            attackScript.SetAttackValues(AttackOwner.Enemy, damage, attackSize, attackKnockBack, attackRange, (attackSpeed * directionFacing), attackHealth);
            attack.transform.localScale = attack.transform.localScale * attackSize * 0.2f;
            //attack.GetComponent<BoxCollider2D>().size = attack.GetComponent<BoxCollider2D>().size * currentSize * 0.2f;
            //attack.GetComponent<Rigidbody2D>().AddForce(new Vector2(directionFacing, 0));

        }
    }

    public void Damaged(int amountDamaged)
    {
        if (invulTimer <= Time.time)
        {
            invulTimer = Time.time + invulDelay;
            health -= amountDamaged;
        }
        if (health <= 0)
        {
            Dead();
        }
    }

    public void Dead()
    {
        switch (GameModeManager.gameModeToLoad)
        {
            case GameModes.Single:
                GameObject.FindWithTag("SPManager").GetComponent<SinglePlayerManager>().EnemyKilled();
                break;
            case GameModes.Coop:
                GameObject.FindWithTag("CPManager").GetComponent<CoopManager>().EnemyKilled();
                break;
            case GameModes.PvP:
                
                break;
        }
        Destroy(gameObject);
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
                break;
            case "Fire":
                animator.SetBool("Walking", false);
                animator.SetBool("Firing", true);
                gameObject.transform.localScale = new Vector3(directionFacing, 1);
                break;
        }
    }

}
