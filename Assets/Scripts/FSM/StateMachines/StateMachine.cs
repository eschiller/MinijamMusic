using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateMachine : MonoBehaviour
{
    public FSMState startingState;
    public FSMState currentState;
    public FSMState previousState;

    public Dictionary<string, FSMState> allStates;

    public StateMachine() {
        allStates = new Dictionary<string, FSMState>();
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (currentState == null) {
            currentState = startingState;
        }

        if (previousState == null) {
            previousState = startingState;
        }

        currentState.UpdateState();
    }

    public virtual void ChangeState(string newState)
    {
        if (currentState != null)
        {
            previousState = currentState;
            currentState.ExitState();
        }
        currentState = allStates[newState];
        currentState.SetStateMachine(this);
        currentState.EnterState();
    }

    public virtual void RevertState()
    {
        FSMState tempState = currentState;
        currentState.ExitState();
        currentState = previousState;
        previousState = tempState;
        currentState.SetStateMachine(this);
        currentState.EnterState();
    }

    public void AddState(string stateName, FSMState s) {
        allStates.Add(stateName, s);
    }
}
