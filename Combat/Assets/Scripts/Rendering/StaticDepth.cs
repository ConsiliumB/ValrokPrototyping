using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticDepth : MonoBehaviour
{
    private SpriteRenderer Renderer;
    public float offsetToActualBase;

    // Use this for initialization
    void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
        Renderer.sortingOrder = (int)((transform.position.y + offsetToActualBase) * -100f);
    }
}
