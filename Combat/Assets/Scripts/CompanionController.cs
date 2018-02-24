using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionController : MonoBehaviour {

    public GameObject player;

    public int proximityLimit;
    public int distanceLimit;

    public GameObject world;
    public Map worldMap;

    private Vector2 headingToPlayer;
    private Vector2 directionToPlayer;
    private List<Coordinate> path;
    private bool moving;
    private Animator animator;
    private new Rigidbody2D rigidbody;

    private float prevDirX;
    private float prevDirY;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        worldMap = world.GetComponent<WorldGen>().nodeMap;
    }
	
	// Update is called once per frame
	void Update () {
        headingToPlayer = player.transform.position - gameObject.transform.position;
        //directionToPlayer = headingToPlayer / headingToPlayer.magnitude;

        if (headingToPlayer.sqrMagnitude > proximityLimit * proximityLimit)
        {
            if (headingToPlayer.sqrMagnitude > distanceLimit * distanceLimit)
            {
                TeleportToPlayer();
                StopCoroutine("MoveTowardsPlayer");
                moving = false;
            }
            else
            {
                StartCoroutine("MoveTowardsPlayer");
            }
        }
    }

    private void TeleportToPlayer()
    {
        rigidbody.MovePosition(player.transform.position);
    }

    IEnumerator MoveTowardsPlayer()
    {
        if (moving)
        {
            yield break;
        }
        var playerposition = WorldGen.PixelToNodeMap(player.transform.position.x, player.transform.position.y);
        var position = WorldGen.PixelToNodeMap(transform.position.x, transform.position.y);
        Debug.Log(playerposition);
        Debug.Log(position);

        path = Pathfinding.GetNodePath((NodeMap)worldMap, position, playerposition);

        if (path == null) yield break;
        path.Remove(path[path.Count - 1]);

        moving = true;
        Coordinate pathNode;
        Vector2 targetPosition;
        Vector2 initialPosition;
        Vector2 interpolatedMovement;

        float interpolation;
        while(path.Count > 0)
        {
            pathNode = path[0];
            path.Remove(pathNode);
            targetPosition = WorldGen.NodeMapToPixel(pathNode);
            initialPosition = transform.position;

            interpolation = 0;
            while (!(interpolation > 1 || (transform.position.x == targetPosition.x && transform.position.y == targetPosition.y)))
            {
                interpolation += Time.smoothDeltaTime / 0.1f;
                interpolatedMovement = Vector2.Lerp(initialPosition, targetPosition, interpolation);
                UpdateAnimation(interpolatedMovement - (Vector2)transform.position);
                rigidbody.MovePosition(interpolatedMovement);
                yield return null;
            }
        }
        UpdateAnimation(Vector2.zero);

        moving = false;
    }

    private void UpdateAnimation(Vector2 heading)
    {
        if (heading.x == 0f && heading.y == 0f)
        {
            animator.SetFloat("LastDirX", prevDirX);
            animator.SetFloat("LastDirY", prevDirY);

            animator.SetBool("Moving", false);
        }
        else
        {
            prevDirX = heading.x;
            prevDirY = heading.y;

            animator.SetBool("Moving", true);
        }

        if (heading.x > 0)
        {
            transform.localScale = Vector3.forward + Vector3.up + Vector3.right;
        }
        else if (heading.x < 0)
        {
            transform.localScale = Vector3.forward + Vector3.up + Vector3.left;
        }

        animator.SetFloat("DirX", heading.x);
        animator.SetFloat("DirY", heading.y);
    }
}
