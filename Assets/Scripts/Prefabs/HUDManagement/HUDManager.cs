using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HUDManager : MonoBehaviour {

    Text middleText;

	// Use this for initialization
	void Start () {
        middleText = transform.Find("TextCanvas/MiddleText").GetComponent<Text>();
        middleText.enabled = false;
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void PauseGame() {
        middleText.text = "Paused";
        middleText.enabled = true;
    }

    public void UnpauseGame()
    {
        middleText.text = "";
        middleText.enabled = false;
    }
}
