using UnityEngine;
using System.Collections;

public class WanderState : FSMState
{
    public string nextState;
    public int speed = 100;
    public Transform parentTransform;
    public float expireTime = 1.0f;

    private float timeCount = 0.0f;

    public WanderState(string nextState, float expireTime, int speed, Transform transform) {
        this.nextState = nextState;
        this.expireTime = expireTime;
        this.speed = speed;
        this.parentTransform = transform;
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
        parentTransform.Translate(Vector2.left * speed * Time.deltaTime);

        timeCount += Time.deltaTime;
        if (timeCount > this.expireTime)
        {
            this.timeCount = 0.0f;
            this.parentMachine.ChangeState(nextState);
        }
    }
}
