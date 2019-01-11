using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerInput : MonoBehaviour {
    float directionInputX, directionInputY;
    public PlatformerController2D myPlatformerPhysics;
    bool jumping;
    bool hasItem;
    bool action1;


	// Use this for initialization
	void Start () {
        hasItem = false;
	}
	
	// Update is called once per frame
	void Update () {
        directionInputX = Input.GetAxisRaw("Horizontal");
        directionInputY = Input.GetAxisRaw("Vertical");
        jumping = Input.GetButtonDown("Jump");
        action1 = Input.GetButtonDown("Fire1");

        if (directionInputY < -.3f) {
            myPlatformerPhysics.Duck();
        } else {
            myPlatformerPhysics.Unduck();
        }

        if (jumping)
        {
            Debug.Log("Space is down");
            myPlatformerPhysics.Jump();
        }

        if (action1) {
            myPlatformerPhysics.Attack();
        }
        myPlatformerPhysics.setActiveXVel(directionInputX);
    }
}
