using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerInput : MonoBehaviour {
    float directionInputX;
    public PlatformerController2D myPlatformerPhysics;
    bool jumping;
    bool hasItem;


	// Use this for initialization
	void Start () {
        hasItem = false;
	}
	
	// Update is called once per frame
	void Update () {
        directionInputX = Input.GetAxisRaw("Horizontal");
        jumping = Input.GetButtonDown("Jump");

        if (jumping)
        {
            Debug.Log("Space is down");
            myPlatformerPhysics.Jump();
        }
        myPlatformerPhysics.setActiveXVel(directionInputX);
    }
}
