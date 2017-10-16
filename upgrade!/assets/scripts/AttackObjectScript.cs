using UnityEngine;
using System.Collections;

public class AttackObjectScript : MonoBehaviour {

    public AttackOwner attackOwner;
    public int damage;
    public int size;
    public int knockback;
    public float range;
    public float rangeTimer;
    public float speed;
    public int health;

    Rigidbody2D rB;
    
	// Use this for initialization
	void Start ()
    {
        rB = gameObject.GetComponent<Rigidbody2D>();
        /*
        if (transform.localScale.x < 0.3)
        {
            transform.localScale = new Vector2(0.3f, transform.localScale.y);
        }
        if (transform.localScale.y < 0.3f)
        {
            transform.localScale = new Vector2(transform.localScale.x, 0.3f);
        }*/
	}
	
	// Update is called once per frame
	void Update ()
    {
        //transform.localPosition += Vector3.right * speed * Time.deltaTime;
        //rB.AddForce(Vector3.forward * speed);
        rB.velocity = (Vector2)transform.TransformDirection(Vector3.down) * speed;
        if (rangeTimer <= Time.time)
        {
            Destroy(gameObject);
        }
	}

    public void SetAttackValues(AttackOwner owner, int _damage, int _size, int _knockback, float _range, int _speed, int _health)
    {
        attackOwner = owner;
        damage = _damage;
        size = _size;
        knockback = _knockback;
        range = _range;
        rangeTimer = Time.time + range;
        speed = _speed;
        health = _health;
    }

    void OnTriggerEnter2D(Collider2D whatIHit)
    {
        if (attackOwner == AttackOwner.Player)
        {
            if (whatIHit.transform.tag == "Enemy")
            {
                whatIHit.GetComponent<EnemyController>().Damaged(damage);
                if (speed < 0)//Check if better way
                {
                    whatIHit.GetComponent<Rigidbody2D>().AddForce(new Vector2(knockback * -1, 0), ForceMode2D.Force);
                }
                else
                {
                    whatIHit.GetComponent<Rigidbody2D>().AddForce(new Vector2(knockback, 0), ForceMode2D.Force);
                }
                health -= 1;
                if (health <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
        else if (attackOwner == AttackOwner.Enemy)
        {
            if (whatIHit.transform.tag == "Player" || whatIHit.transform.tag == "Player1" || whatIHit.transform.tag == "Player2")
            {
                whatIHit.GetComponent<PlayerController>().Damaged(damage);
                if (speed < 0)//Check if better way
                {
                    whatIHit.GetComponent<Rigidbody2D>().AddForce(new Vector2(knockback * -1, 0), ForceMode2D.Force);
                }
                else
                {
                    whatIHit.GetComponent<Rigidbody2D>().AddForce(new Vector2(knockback, 0), ForceMode2D.Force);
                }
                health -= 1;
                if (health <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
        else if (attackOwner == AttackOwner.Player1)
        {
            if (whatIHit.transform.tag == "Player2")
            {
                whatIHit.GetComponent<PlayerController>().Damaged(damage);
                if (speed < 0)//Check if better way
                {
                    whatIHit.GetComponent<Rigidbody2D>().AddForce(new Vector2(knockback * -1, 0), ForceMode2D.Force);
                }
                else
                {
                    whatIHit.GetComponent<Rigidbody2D>().AddForce(new Vector2(knockback, 0), ForceMode2D.Force);
                }
                health -= 1;
                if (health <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
        else if (attackOwner == AttackOwner.Player2)
        {
            if (whatIHit.transform.tag == "Player1")
            {
                whatIHit.GetComponent<PlayerController>().Damaged(damage);
                if (speed < 0)//Check if better way
                {
                    whatIHit.GetComponent<Rigidbody2D>().AddForce(new Vector2(knockback * -1, 0), ForceMode2D.Force);
                }
                else
                {
                    whatIHit.GetComponent<Rigidbody2D>().AddForce(new Vector2(knockback, 0), ForceMode2D.Force);
                }
                health -= 1;
                if (health <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }

        if (whatIHit.tag == "Platform")
        {
            Destroy(gameObject);
        }
    }

}

public enum AttackOwner { Player, Enemy, Player1, Player2 }
