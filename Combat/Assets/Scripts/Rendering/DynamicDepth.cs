using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicDepth : MonoBehaviour
{
    private SpriteRenderer Renderer;
    public float offsetToActualBase;

    void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
        Renderer.sortingOrder = (int)((transform.position.y + offsetToActualBase) * -100f);
    }

    private void Update()
    {
        Renderer.sortingOrder = (int)((transform.position.y + offsetToActualBase) * -100f);
    }
}
