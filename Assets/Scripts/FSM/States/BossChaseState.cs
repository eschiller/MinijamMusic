using UnityEngine;
using System.Collections;

public class BossChaseState : FSMState
{
    public int speed = 100;
    public float maximumDistance = 100f;
    public OverheadController2D charController;
    public CharacterManager charManager;


    private int walkDirection = 1;
    private System.Random rnd;

    public BossChaseState(int speed,
                       float maximumDistance, 
                       OverheadController2D controller, CharacterManager cm) {
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

        //if somehow we're here with no target, go back to the last state
        if (charManager.targetTransform == null) {
            this.machine.RevertState();
        }

        charController.Attack("down");

        //if the target has gotten too far, untarget and move back to idle
        float dis = Vector3.Distance(charController.transform.position, charManager.targetTransform.position);

        if (dis > maximumDistance) {
            Debug.Log("Lost target");
            charManager.targetTransform = null;
            machine.ChangeState("idle");
        }

        //figure out which direction to walk to go towards target X
        float dirx = 0f;
        float distanceThreshold = charController.speed / 8;
        float relativeX = charController.transform.position.x - charManager.targetTransform.position.x;
        if (relativeX < distanceThreshold * -1) {
            dirx = 1f;
        }
        if (relativeX > distanceThreshold)
        {
            dirx = -1f;
        }

        charController.setActiveXVel(dirx);


        //figure out which direction to walk to go towards target Y
        /*
        float diry = 0f;
        float relativeY = charController.transform.position.y - charManager.targetTransform.position.y;
        if (relativeY < distanceThreshold * -1)
        {
            diry = 1f;
        }
        if (relativeY > distanceThreshold)
        {
            diry = -1f;
        }

        charController.setActiveYVel(diry);
        */
    }
}
