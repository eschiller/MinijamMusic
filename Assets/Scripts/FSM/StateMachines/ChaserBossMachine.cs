using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChaserBossMachine : StateMachine {
    public int wanderStateSpeed = 100;


    public void Start()
    {
        CharacterManager cm = GetComponent<CharacterManager>();
        OverheadController2D control = GetComponent<OverheadController2D>();

        AddState("wander", new BossWanderState("idle", 1.0f, wanderStateSpeed, control));
        AddState("idle", new BossIdleState("patrol", 2.0f));
        AddState("patrol", new BossPatrolState(200f, wanderStateSpeed, 350f, "Player", control, cm));
        AddState("chase", new BossChaseState(wanderStateSpeed, 400f, control, cm));
        ChangeState("idle");
    }

}
