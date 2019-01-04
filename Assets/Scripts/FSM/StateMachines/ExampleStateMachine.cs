using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExampleStateMachine : StateMachine {
    public int wanderStateSpeed = 100;


    public void Start()
    {
        AddState("wander", new WanderState("idle", 1.0f, wanderStateSpeed, this.transform));
        AddState("idle", new IdleState("wander", 1.0f));

        ChangeState("wander");
    }

}
