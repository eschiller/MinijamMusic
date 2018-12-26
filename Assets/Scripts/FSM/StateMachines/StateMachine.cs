using UnityEngine;
using System.Collections;

public class StateMachine : MonoBehaviour {
	public IState startingState;
	private IState currentState;
	private IState previousState;

    private void Start()
    {
        currentState = startingState;
        changeState(startingState);
    }

    public virtual void changeState(IState newState) {
		previousState = currentState;
		currentState.exitState ();
		currentState = newState;
        currentState.setStateMachine(this);
		currentState.enterState ();
	}

	public virtual void revertState() {
		IState tempState = currentState;
		currentState.exitState ();
		currentState = previousState;
		previousState = tempState;
        currentState.setStateMachine(this);
		currentState.enterState ();
	}
}
