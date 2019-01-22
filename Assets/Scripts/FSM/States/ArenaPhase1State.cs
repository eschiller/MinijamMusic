using UnityEngine;
using System.Collections;

public class ArenaPhase1State : FSMState
{
    LevelManager levelManager;
    GameObject mob1;
    string nextState;

    public ArenaPhase1State(string nextState, LevelManager levelManager, GameObject mob1)
    {
        this.nextState = nextState;
        this.levelManager = levelManager;
        this.mob1 = mob1;
    }

    public override FSMState CheckForNewState()
    {
        return null;
    }

    public override void EnterState()
    {
        levelManager.SpawnEnemy("mob1", mob1, new Vector3(-250, 22, 0));
        levelManager.SpawnEnemy("mob2", mob1, new Vector3(260, 22, 0));
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        if (levelManager.GetEnemyCount() == 0) {
            machine.ChangeState(nextState);
        }
    }
}

