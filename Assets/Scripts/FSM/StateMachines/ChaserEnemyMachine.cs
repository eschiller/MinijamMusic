using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChaserEnemyMachine : StateMachine {
    public int wanderStateSpeed = 100;


    public void Start()
    {
        CharacterManager cm = GetComponent<CharacterManager>();
        OverheadController2D control = GetComponent<OverheadController2D>();

        AddState("wander", new WanderState("idle", 1.0f, wanderStateSpeed, control));
        AddState("idle", new IdleState("patrol", 2.0f));
        AddState("patrol", new PatrolState(10f, wanderStateSpeed, 200f, "Player", control, cm));
        AddState("chase", new ChaseState(wanderStateSpeed, 300f, control, cm));
        ChangeState("wander");
    }

}
