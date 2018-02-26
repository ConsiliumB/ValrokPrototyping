using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatefulEntity : MonoBehaviour {

    public IState currentState;

    public void ChangeState(IState newState)
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
