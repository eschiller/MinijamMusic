using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSceneInput : MonoBehaviour {
    bool hitEnter;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        hitEnter = Input.GetButtonDown("Submit");

        if (hitEnter) {
            GameObject gm = GameObject.Find("GameManager");
            gm.GetComponent<GameManager>().ChangeScene("floor1");
        }
    }
}
