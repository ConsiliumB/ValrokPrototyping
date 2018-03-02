using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatefulEntity : MonoBehaviour
{
    public State currentState;
    public Coordinate Position
    {
        get {
            return WorldGen.PixelToNodeMap(gameObject.transform.position.x, gameObject.transform.position.y);
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
