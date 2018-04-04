using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {
    public GameObject attacker;
    public bool friendlyFire;

    private void Start()
    {
        Destroy(gameObject, 3.0f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StatefulEntity entity = collision.gameObject.GetComponent<StatefulEntity>();
        if (entity)
        {
            if(entity.mortal && (friendlyFire || entity.gameObject.layer != attacker.gameObject.layer))
            {
                entity.Die();
                Destroy(gameObject);
            }
        }
    }
}
