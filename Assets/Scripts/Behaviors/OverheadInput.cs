using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverheadInput : MonoBehaviour {
    float directionInputX, directionInputY;
    public OverheadController2D myOverheadController;
    bool hasItem;
    bool action1, action2, action3, action4;


	// Use this for initialization
	void Start () {
        hasItem = false;
	}
	
	// Update is called once per frame
	void Update () {
        directionInputX = Input.GetAxisRaw("Horizontal");
        directionInputY = Input.GetAxisRaw("Vertical");
        action1 = Input.GetButtonDown("Fire1");
        action2 = Input.GetButtonDown("Fire2");
        action3 = Input.GetButtonDown("Fire3");
        action4 = Input.GetButtonDown("Jump");


        if (action1) {
            myOverheadController.Attack("up");
        }
        if (action2)
        {
            myOverheadController.Attack("right");
        }
        if (action3)
        {
            myOverheadController.Attack("down");
        }
        if (action4)
        {
            myOverheadController.Attack("left");
        }
        myOverheadController.setActiveXVel(directionInputX);
        myOverheadController.setActiveYVel(directionInputY);
    }
}
