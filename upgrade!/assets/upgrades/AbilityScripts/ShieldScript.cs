using UnityEngine;
using System.Collections;

public class ShieldScript : MonoBehaviour {

    public float deathTimer;

	// Use this for initialization
	void Start ()
    {
        deathTimer = Time.time + 5;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (deathTimer <= Time.time)
        {
            Destroy(gameObject);
        }
	}

    void OnTriggerEnter2D(Collider2D whatIHit)
    {
        if (whatIHit.transform.tag == "Projectile")
        {
            Destroy(whatIHit.gameObject);
            //Destroy(gameObject);
        }
    }
}
