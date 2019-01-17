using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class FSMState 
{
    public StateMachine machine ;


    abstract public FSMState CheckForNewState();
    abstract public void ExitState();
    abstract public void EnterState();
    abstract public void UpdateState();

    public void SetStateMachine(StateMachine m) {
        this.machine = m;
    }
}
