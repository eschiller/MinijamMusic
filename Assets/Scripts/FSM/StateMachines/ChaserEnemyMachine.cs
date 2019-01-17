using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RedBlobStateMachine : StateMachine {
    public int wanderStateSpeed = 100;


    public void Start()
    {
        CharacterManager cm = GetComponent<CharacterManager>();
        PlatformerController2D control = GetComponent<PlatformerController2D>();

        AddState("wander", new WanderState("idle", 1.0f, wanderStateSpeed, control));
        AddState("idle", new IdleState("patrol", 2.0f));
        AddState("patrol", new PatrolState(10f, wanderStateSpeed, 200f, "Player", control, cm));
        AddState("chase", new ChaseState(wanderStateSpeed, 300f, control, cm));
        ChangeState("wander");
    }

}
