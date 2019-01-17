using UnityEngine;
using System.Collections;

public class IdleState : FSMState
{
    public string nextState;
    public float expireTime = 1.0f;
    private float timeCount = 0.0f;

    public Transform transform;

    public IdleState(string nextState, float expireTime)
    {
        this.nextState = nextState;
        this.expireTime = expireTime;
    }

    public override FSMState CheckForNewState()
    {
        return null;
    }

    public override void EnterState()
    {
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        timeCount += Time.deltaTime;
        if (timeCount > this.expireTime) {
            this.timeCount = 0.0f;
            this.machine.ChangeState(nextState);
        }
    }
}
