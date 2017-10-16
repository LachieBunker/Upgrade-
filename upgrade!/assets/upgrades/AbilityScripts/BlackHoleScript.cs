using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlackHoleScript : MonoBehaviour {

    Rigidbody2D rB;
    public float speed;
    public bool activated;
    public GameObject owner;
    public List<GameObject> attractedObjects;
    public GameObject sprite;

	// Use this for initialization
	void Start ()
    {
        rB = gameObject.GetComponent<Rigidbody2D>();
        activated = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!activated)
        {
            rB.velocity = (Vector2)transform.TransformDirection(Vector3.down) * speed;
        }
        else
        {
            rB.velocity = Vector2.zero;
            for (int i = 0; i < attractedObjects.Count; i++)
            {
                if (attractedObjects[i] != null)
                {
                    attractedObjects[i].transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
                }
            }
        }

        sprite.transform.Rotate(0, 0, -1);
    }

    public void SetSpeed(int _speed)
    {
        speed = _speed;
    }

    public void ReleaseObjects()
    {
        for (int i = 0; i < attractedObjects.Count; i++)
        {
            if (attractedObjects[i] != null)
            {
                attractedObjects[i].GetComponent<Rigidbody2D>().gravityScale = 1;
				if(attractedObjects[i].transform.tag == "Enemy")
                {
                   	attractedObjects[i].GetComponent<EnemyController>().stuck = false;
                }
            }
        }
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(2);
        ReleaseObjects();
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D whatIHit)
    {
        if (activated == false)
        {
            if (whatIHit.transform.tag != owner.transform.tag)
            {
                activated = true;
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * 2, gameObject.transform.localScale.y * 2);
                StartCoroutine(DestroyObject());
				switch (whatIHit.transform.tag)
	            {
	                case "Player":
	                case "Player1":
	                case "Player2":
	                case "Enemy":
	                case "Projectile":
	                    whatIHit.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
	                    whatIHit.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
	                    if(whatIHit.transform.tag == "Enemy")
	                    {
	                    	whatIHit.gameObject.GetComponent<EnemyController>().stuck = true;
	                    }
	                    attractedObjects.Add(whatIHit.gameObject);
	                    break;
	            }
            }
            
        }
        else
        {
            switch (whatIHit.transform.tag)
            {
                case "Player":
                case "Player1":
                case "Player2":
                case "Enemy":
                case "Projectile":
                    whatIHit.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
                    whatIHit.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                    if(whatIHit.transform.tag == "Enemy")
                    {
                    	whatIHit.gameObject.GetComponent<EnemyController>().stuck = true;
                    }
                    attractedObjects.Add(whatIHit.gameObject);
                    break;
            }
        }
    }
}
