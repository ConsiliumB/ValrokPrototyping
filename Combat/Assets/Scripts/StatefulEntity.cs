using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatefulEntity : MonoBehaviour {

    public State currentState;

	private void Update () {
        currentState.Execute();
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
