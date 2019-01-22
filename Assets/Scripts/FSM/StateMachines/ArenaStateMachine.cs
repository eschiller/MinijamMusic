using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaStateMachine : LevelStateMachine {
    public GameObject mob1;

	// Use this for initialization
	void Start () {
        AddState("phase1", new ArenaPhase1State("phase2", levelManager, mob1));
        AddState("phase2", new ArenaPhase2State("phase2", levelManager, mob1));

        ChangeState("phase1");
	}

}
