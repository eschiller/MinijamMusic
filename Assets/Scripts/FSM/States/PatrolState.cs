using UnityEngine;
using System.Collections;

/*
 * Requires a state with key "chase", and a key "idle" in the state machine
 */
public class PatrolState : FSMState
{
    public int speed = 100;
    public PlatformerController2D charController;
    public CharacterManager charManager;
    public float expireTime = 1.0f;
    public string attackTarget = "Player";
    public float minimumDistance = 50.0f;
    private int walkDirection = 1;
    private float timeCount = 0.0f;

    private System.Random rnd;
    private bool foundTarget;

    public PatrolState(float expireTime, int speed, 
                       float minimumDistance, string attackTarget, 
                       PlatformerController2D controller, CharacterManager cm) {
        rnd = new System.Random();

        this.expireTime = expireTime;
        this.speed = speed;
        this.charController = controller;
        this.attackTarget = attackTarget;
        this.minimumDistance = minimumDistance;
        this.charManager = cm;
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


    public void LookForTarget() {
        GameObject[] attackTargets;
        attackTargets = GameObject.FindGameObjectsWithTag(attackTarget);
        foreach (GameObject at in attackTargets) {
            float dis = Vector3.Distance(at.transform.position, charController.transform.position);
            Debug.Log("found potential target " + dis + " far away");

            if (dis < minimumDistance)
            {
                Debug.Log("FOUND A TARGET!");
                foundTarget = true;
                charManager.SetTargetTransform(at.transform);
            } else {
                foundTarget = false;
            }
        }
    }


    public override void UpdateState()
    {
        Debug.Log("In patrol state");

        charController.setActiveXVel(walkDirection);

        LookForTarget();

        if(foundTarget) {
            this.machine.ChangeState("chase");
        }

        timeCount += Time.deltaTime;
        if (timeCount > this.expireTime)
        {
            this.timeCount = 0.0f;
            this.machine.ChangeState("idle");
        }
    }
}
