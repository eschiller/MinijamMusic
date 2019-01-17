using UnityEngine;
using System.Collections;

public class ChaseState : FSMState
{
    public int speed = 100;
    public float maximumDistance = 100f;
    public PlatformerController2D charController;
    public CharacterManager charManager;


    private int walkDirection = 1;
    private System.Random rnd;

    public ChaseState(int speed,
                       float maximumDistance, 
                       PlatformerController2D controller, CharacterManager cm) {
        rnd = new System.Random();

        //speed does nothing
        this.speed = speed;
        this.maximumDistance = maximumDistance;
        this.charController = controller;
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

    public override void UpdateState()
    {
        Debug.Log("In chase state");

        //if somehow we're here with no target, go back to the last state
        if (charManager.targetTransform == null) {
            this.machine.RevertState();
        }

        //if the target has gotten too far, untarget and move back to idle
        float dis = Vector3.Distance(charController.transform.position, charManager.targetTransform.position);
        Debug.Log("Target is " + dis + " distance");

        if (dis > maximumDistance) {
            Debug.Log("Lost target");
            charManager.targetTransform = null;
            machine.ChangeState("idle");
        }

        //figure out which direction to walk to go towards target
        float dir = 0f;
        float distanceThreshold = charController.speed / 8;
        float relativeX = charController.transform.position.x - charManager.targetTransform.position.x;
        if (relativeX < distanceThreshold * -1) {
            Debug.Log("target is right");
            dir = 1f;
        }
        if (relativeX > distanceThreshold)
        {
            Debug.Log("target is left");
            dir = -1f;
        }

        charController.setActiveXVel(dir);
    }
}
