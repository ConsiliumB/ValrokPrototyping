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
        float hmov = Input.GetAxis("Horizontal");
        rb2d.MoveRotation(rb2d.rotation - hmov * speed * 5 * Time.fixedDeltaTime);
        rb2d.velocity = transform.up * vmov * speed / 2;
    }

    private void Shoot()
    {
        var bullet = (GameObject)Instantiate(projectile, transform.position, transform.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = transform.right * -1 * 10;

        Destroy(bullet, 2.0f);
    }
}
