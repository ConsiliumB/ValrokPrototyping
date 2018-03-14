using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatefulEntity : MonoBehaviour
{
    public delegate void PositionUpdateHandler();
    public event PositionUpdateHandler PositionUpdate;

    public State currentState;
    public Coordinate PreviousPosition;
    public Coordinate Position
    {
        get {
            return WorldGen.PixelToNodeMap(gameObject.transform.position.x, gameObject.transform.position.y);
        }
    }


    //Checks for a position change and calls a positionupdate event if subgrid position has changed
    IEnumerator CheckPositionChange()
    {
        for (;;)
        {
            yield return new WaitForSeconds(0.3f);

            if (PositionUpdate != null)
            {
                if (Pathfinding.Graph.WithinBounds(Position))
                {
                    if (PreviousPosition != Position)
                    {
                        PreviousPosition = Position;
                        PositionUpdate();
                    }
                }
            }
        }
    }

    public void ChangeState(State newState)
    {
        if (currentState != null)
        {
            currentState.FinishState();
        }

        currentState = newState;

        if (currentState != null)
        {
            currentState.PrepareState();
        }
    }
}
