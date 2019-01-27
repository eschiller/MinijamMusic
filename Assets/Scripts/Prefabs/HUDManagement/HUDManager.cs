using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HUDManager : MonoBehaviour {

    Text middleText;
    public GameObject healthIcon;
    GameObject allCanvas;

    private List<GameObject> allHealth;

    public  HUDManager() {
        allHealth = new List<GameObject>();
    }

	// Use this for initialization
	void Start () {
        allCanvas = GameObject.Find("TextCanvas");
        middleText = transform.Find("TextCanvas/MiddleText").GetComponent<Text>();
        middleText.enabled = false;
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void SetMiddleText(string sometext) {
        middleText.text = sometext;
        middleText.enabled = true;
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


    public void SetHealth(int health, int xPadding = 16, int yPadding = -16, int spacing = 20) {
        foreach(GameObject hIcon in allHealth) {
            Destroy(hIcon);
        }

        for (int i = 0; i < health; i++) {
            GameObject aHeart;
            aHeart = Instantiate(healthIcon, new Vector3((xPadding + (spacing * i)), yPadding, -1), Quaternion.identity);
            aHeart.transform.SetParent(allCanvas.transform, false);
            Debug.Log(aHeart.transform.name);
            allHealth.Add(aHeart);
        }
    }
}
