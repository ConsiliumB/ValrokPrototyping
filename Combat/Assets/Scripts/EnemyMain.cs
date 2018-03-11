using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMain : MonoBehaviour {

    [Header("Enemy lives")]
    public int lives;
    [Space]
    public float hitDelay;

    private int t;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage()
    {
        lives--;
        if (lives <= 0)
        {
            //RunDeathAnimation
            Destroy(gameObject, 0.1f);
        }
    }

    private void OnDestroy()
    {
        transform.parent.gameObject.GetComponent<CorruptionSpawner>().EnemyDied();
    }

    //On collision with other gameobjects 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //
    }

    //When a trigger is entered
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CompanionAttack"))
        {
            TakeDamage();
        }
    }
}
