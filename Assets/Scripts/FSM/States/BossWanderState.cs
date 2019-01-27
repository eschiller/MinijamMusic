using UnityEngine;
using System.Collections;

public class BossWanderState : FSMState
{
    public string nextState;
    public int speed = 100;
    public OverheadController2D parentController;
    public float expireTime = 1.0f;
    private int walkDirection = 1;
    private float timeCount = 0.0f;

    private System.Random rnd;

    public BossWanderState(string nextState, float expireTime, int speed, OverheadController2D controller) {
        rnd = new System.Random();

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
        walkDirection = rnd.Next(-1, 2);
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        parentController.setActiveXVel(walkDirection);

        timeCount += Time.deltaTime;
        if (timeCount > this.expireTime)
        {
            this.timeCount = 0.0f;
            this.machine.ChangeState(nextState);
        }
    }
}
