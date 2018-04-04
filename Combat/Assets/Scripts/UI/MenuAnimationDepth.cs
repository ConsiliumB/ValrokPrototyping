using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimationDepth : MonoBehaviour
{
    private SpriteRenderer Renderer;
    public float offsetToActualBase;
    public bool appendToChildren = false;

    public void SetDepth()
    {
        Renderer = GetComponent<SpriteRenderer>();
        Renderer.sortingOrder = (int)((transform.position.y + offsetToActualBase) * -100f);

        if (appendToChildren)
        {
            SpriteRenderer[] childRenderers = GetComponentsInChildren<SpriteRenderer>();

            foreach (SpriteRenderer item in childRenderers)
            {
                item.sortingOrder = Renderer.sortingOrder + 30 - (int)(item.transform.position.y*10);
            }
        }
    }
}
