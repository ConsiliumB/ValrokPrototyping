using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoilageTransparency : MonoBehaviour {
    [Range(0,1)]
    public float transparency;

    private int overlappingObjects = 0;
    private new SpriteRenderer renderer;
    private Color transparentColor;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        transparentColor = new Color(1f, 1f, 1f, transparency);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != gameObject.layer)
        {
            overlappingObjects++;

            if (overlappingObjects == 1)
            {
                renderer.color = transparentColor;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer != gameObject.layer)
        {
            overlappingObjects--;

            if (overlappingObjects == 0)
            {
                renderer.color = Color.white;
            }
        }
    }
}
