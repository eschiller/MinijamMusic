using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSizer : MonoBehaviour {
    private AmplitudeSampler myAmplitudeSampler;


	// Use this for initialization
	void Start () {
        myAmplitudeSampler = GameObject.Find("AudioPlayer").GetComponent<AmplitudeSampler>();
	}
	
	// Update is called once per frame
	void Update () {

        transform.localScale = new Vector3(1, myAmplitudeSampler.GetMusicAmplitude() * 10f);
	}
}
