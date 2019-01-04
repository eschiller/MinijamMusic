using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExampleStateMachine : StateMachine {
    public int wanderStateSpeed = 100;


    public void Start()
    {
        AddState("wander", new WanderState("idle", 1.0f, wanderStateSpeed, this.GetComponent<PlatformerController2D>()));
        AddState("idle", new IdleState("wander", 10.0f));

        ChangeState("wander");
    }

}
