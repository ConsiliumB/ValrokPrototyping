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
            Destroy(gameObject, 1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "CompanionAttack")
        {
            TakeDamage();
        }
    }
}
