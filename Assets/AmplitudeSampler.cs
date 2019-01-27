using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmplitudeSampler : MonoBehaviour {
    private AudioSource myAudioSource;
    private int sampleLength = 1024;
    float updateInterval = .05f;
    float currentUpdateTime = 0f;
    private float[] clipSampleData;
    float currentAmp = 0f;

	// Use this for initialization
	void Start () {
        myAudioSource = GetComponent<AudioSource>();
        clipSampleData = new float[1024];
	}
	
	// Update is called once per frame
	void Update () {
        currentUpdateTime += Time.deltaTime;
        if (currentUpdateTime > updateInterval) {
            //reset timer
            currentUpdateTime = 0f;

            myAudioSource.clip.GetData(clipSampleData, myAudioSource.timeSamples);

            currentAmp = 0f;
            foreach(var sample in clipSampleData) {
                currentAmp += Mathf.Abs(sample);
            }
            currentAmp /= sampleLength;

            //Debug.Log("current amp is " + currentAmp);
        }
	}

    public float GetMusicAmplitude() {
        return currentAmp;
    }
}
