using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IState {
	void updateState ();
	void setTransitions(Dictionary<string, IState> transitionStates);
	void setStateMachine(StateMachine theMachine);
	void enterState();
	void exitState();
}
