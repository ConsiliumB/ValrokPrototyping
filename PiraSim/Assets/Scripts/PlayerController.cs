using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public int speed;
    public GameObject projectile;
    private Rigidbody2D rb2d;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        float vmov = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(0, vmov);
        rb2d.AddForce(movement * speed);
    }

    private void Shoot()
    {
        var bullet = (GameObject)Instantiate(projectile, transform.position, transform.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = Vector2.left * 10;

        Destroy(bullet, 2.0f);
    }
}
