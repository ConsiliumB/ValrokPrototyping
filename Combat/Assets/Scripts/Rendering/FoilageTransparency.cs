using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoilageTransparency : MonoBehaviour {
    [Range(0,1)]
    public float transparency;

    private int overlappingObjects = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != gameObject.layer)
        {
            overlappingObjects++;

            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, transparency);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer != gameObject.layer)
        {
            overlappingObjects--;

            if (overlappingObjects == 0)
            {
                SpriteRenderer renderer = GetComponent<SpriteRenderer>();
                renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1f);
            }
        }
    }
}
