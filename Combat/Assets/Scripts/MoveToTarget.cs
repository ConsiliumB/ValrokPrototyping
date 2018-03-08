using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTarget : MonoBehaviour
{

    private GameObject target;
    public List<Coordinate> path;
    //private CompanionMovement moveScript = new CompanionMovement();
    private bool done = false;
    private float counter = 0;
    private float delay = 0.1f;

    //##CompanionMovement copy
    private Coordinate pathNode;
    Vector2 targetPosition;
    Vector2 initialPosition;
    Vector2 interpolatedMovement;
    float Interpolation { get; set; }
    //##End CompanionMovement copy

    // Use this for initialization
    // Note: Script is initialized at runtime so we need to find player
    // Or set player when we instansiate enemies ? 
    void Start()
    {
        target = PlayerController.Instance.gameObject;

        var destination = WorldGen.PixelToNodeMap(target.transform.position);
        var from = WorldGen.PixelToNodeMap(transform.position);
        path = Pathfinding.GetPath(Pathfinding.Graph, from, destination);
    }

    // Update is called once per frame
    void Update()
    {
        if (!done)
        {
            /*if (counter < delay) {
                counter += Time.deltaTime;
            } else { */
                MoveAlongPath();
                counter = 0;
            //}
        }
    }

    // Hacky method to make MoveAlngPath work when called without CompanionController
    //Taken from CompanionMovement.cs. Should probably inherit or extend
    void MoveAlongPath()
    {
        if (path == null || path.Count <= 1)
        {
            done = true;
            Debug.Log("Done");
            return;
        }
        
        if (pathNode != path[0])
        {
            pathNode = path[0];
            Interpolation = 0;

            targetPosition = WorldGen.NodeMapToPixel(pathNode);
            initialPosition = transform.position;
        }
        Interpolation += Time.smoothDeltaTime / 0.1f;

        //Find next destination
        interpolatedMovement = Vector2.Lerp(initialPosition, targetPosition, Interpolation);
        transform.position = interpolatedMovement;

        //If we've reached our destination, reset interpolation and remove the path we just reached
        if (Interpolation >= 1)
        {
            Interpolation = 0;
            path.Remove(pathNode);
        }
    }
}
