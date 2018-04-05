using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiseAnimation : MonoBehaviour {

    Vector2 targetPos;
    new SpriteRenderer renderer;
    SpriteRenderer[] children;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        renderer.color = Color.clear;

        targetPos = transform.position;
        transform.position = transform.position - new Vector3(0, 4);
    }

    public void SpawnBlock()
    {
        children = GetComponentsInChildren<SpriteRenderer>();

        StartCoroutine(Rise());
    }

    IEnumerator Rise()
    {
        var timer = 0f;
        Color fadeInColor;

        while (timer < 1.3f)
        {
            timer += Time.smoothDeltaTime;

            var progress = timer / 4f;
            var value = MenuBackgroundController.Instance.easeInCurve.Evaluate(progress);
            var transparency = value * 10f;
            fadeInColor = new Color(transparency, transparency, transparency, transparency);

            renderer.color = fadeInColor;

            foreach (var item in children)
            {
                item.color = fadeInColor;
            }
            transform.position = Vector2.Lerp(transform.position, targetPos, value);
            yield return null;
        }
        //renderer.sortingLayerName = "Floor";
        //foreach (var item in children)
        //{
        //    item.sortingLayerName = "Floor";
        //}
        transform.position = targetPos;
    }
}
