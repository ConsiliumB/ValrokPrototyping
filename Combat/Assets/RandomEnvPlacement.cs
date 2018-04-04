using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnvPlacement : MonoBehaviour
{

    public List<GameObject> trees = new List<GameObject>();
    public List<GameObject> bushes = new List<GameObject>();
    public List<GameObject> tinyfoilage = new List<GameObject>();


    void Start()
    {
        if (Mathf.PerlinNoise(transform.position.x / 12f, transform.position.y / 6f) > 0.7 || Mathf.PerlinNoise(transform.position.x / 12f + 60f, transform.position.y / 6f + 30f) > 0.7)
        {
            Vector2 position = (Vector2)transform.position + new Vector2(Random.Range(0.3f, 1.2f), Random.Range(0.3f, 2.7f));
            Instantiate(trees[Random.Range(0, trees.Count)], position, Quaternion.identity, transform);
        }

        GetComponent<MenuAnimationDepth>().SetDepth();
        GetComponent<RiseAnimation>().SpawnBlock();
    }
}
