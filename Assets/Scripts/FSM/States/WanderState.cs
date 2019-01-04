using UnityEngine;
using System.Collections;

public class WanderState : FSMState
{
    public string nextState;
    public int speed = 100;
    public PlatformerPhysics parentController;
    public float expireTime = 1.0f;

    private float timeCount = 0.0f;

    public WanderState(string nextState, float expireTime, int speed, PlatformerPhysics controller) {
        this.nextState = nextState;
        this.expireTime = expireTime;
        this.speed = speed;
        this.parentController = controller;
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
        parentController.setActiveXVel(-1);

        timeCount += Time.deltaTime;
        if (timeCount > this.expireTime)
        {
            Debug.Log("time expired, changing to idle");
            this.timeCount = 0.0f;
            this.parentMachine.ChangeState(nextState);
        }
    }
}
