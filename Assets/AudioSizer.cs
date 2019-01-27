using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSizer : MonoBehaviour {
    private AmplitudeSampler myAmplitudeSampler;
    private SpriteRenderer mySpriteRenderer;

    private bool isPaused = false;

	// Use this for initialization
	void Start () {
        myAmplitudeSampler = GameObject.Find("AudioPlayer").GetComponent<AmplitudeSampler>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!isPaused)
        {
            transform.localScale = new Vector3(1, myAmplitudeSampler.GetMusicAmplitude() * 10f);
        }
	}


    public void Pause() 
    {
        isPaused = true;
        mySpriteRenderer.color = Color.red;

    }

    public void Unpause() {
        isPaused = false;
        mySpriteRenderer.color = Color.white;

    }
}
