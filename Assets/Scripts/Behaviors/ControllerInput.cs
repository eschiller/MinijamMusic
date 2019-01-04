using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInput : MonoBehaviour {
    float directionInputX;
    public Controller2D controller2D;
    bool jumping;
    bool hasItem;


	// Use this for initialization
	void Start () {
        hasItem = false;
	}
	
	// Update is called once per frame
	void Update () {
        directionInputX = Input.GetAxisRaw("Horizontal");
        //jumping = Input.GetKeyDown("space");
        jumping = Input.GetButtonDown("Jump");


        if (Input.GetKeyDown(KeyCode.E)) {
            Debug.Log("KEY DOWN E");
            if (hasItem)
            {
                Debug.Log("threw item in input");
                controller2D.throwItem();
            }
            else
            {
                if (controller2D.grabItem())
                {
                    Debug.Log("grabbed item in input");
                    hasItem = true;
                }
            }
        }

        if (jumping)
        {
            Debug.Log("Space is down");
        }
        controller2D.movement(directionInputX, jumping);
    }
}
