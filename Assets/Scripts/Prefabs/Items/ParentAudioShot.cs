using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentAudioShot : MonoBehaviour {
    private AmplitudeSampler myAmplitudeSampler;
    public int attackDamage = 1;
    public string target = "Enemy";
    public float lastingTime = 5f;
    public float ySpeed = 2f;
    public float xSpeed = 0f;
    public GameObject subshot;

    private float timeToDeath = 0.0f;
    private float amplitude = 0f;
    private AudioSizer myAudioSizer;

    private bool alreadyUnpaused = false;

	// Use this for initialization
	void Start () {
        myAmplitudeSampler = GameObject.Find("AudioPlayer").GetComponent<AmplitudeSampler>();
        amplitude = myAmplitudeSampler.GetMusicAmplitude();
        myAudioSizer = GameObject.Find("AudioLevel").GetComponent<AudioSizer>();
        myAudioSizer.Pause();

        if ((ySpeed > 1f) || (ySpeed < -1f))
        {
            if (amplitude > .1)
            {
                Debug.Log("creating lvl 2 y shots");
                //left subshot
                GameObject leftSubshot = Instantiate(subshot, transform.position, Quaternion.identity);
                leftSubshot.GetComponent<AudioShot>().xSpeed = -.2f;
                leftSubshot.GetComponent<AudioShot>().ySpeed = ySpeed * .95f;

                GameObject rightSubshot = Instantiate(subshot, transform.position, Quaternion.identity);
                rightSubshot.GetComponent<AudioShot>().xSpeed = .2f;
                rightSubshot.GetComponent<AudioShot>().ySpeed = ySpeed * .95f;
            }

            if (amplitude > .2)
            {
                Debug.Log("creating lvl 3 y shots");
                //left subshot
                GameObject leftSubshot = Instantiate(subshot, transform.position, Quaternion.identity);
                leftSubshot.GetComponent<AudioShot>().xSpeed = -.4f;
                leftSubshot.GetComponent<AudioShot>().ySpeed = ySpeed * .90f;

                GameObject rightSubshot = Instantiate(subshot, transform.position, Quaternion.identity);
                rightSubshot.GetComponent<AudioShot>().xSpeed = .4f;
                rightSubshot.GetComponent<AudioShot>().ySpeed = ySpeed * .90f;
            }

            if (amplitude > .3)
            {
                Debug.Log("creating lvl 3 y shots");
                //left subshot
                GameObject leftSubshot = Instantiate(subshot, transform.position, Quaternion.identity);
                leftSubshot.GetComponent<AudioShot>().xSpeed = -.6f;
                leftSubshot.GetComponent<AudioShot>().ySpeed = ySpeed * .85f;

                GameObject rightSubshot = Instantiate(subshot, transform.position, Quaternion.identity);
                rightSubshot.GetComponent<AudioShot>().xSpeed = .6f;
                rightSubshot.GetComponent<AudioShot>().ySpeed = ySpeed * .85f;
            }
        }
        if ((xSpeed > .8f) || (xSpeed < -.8f)) {
            if (amplitude > .1)
            {
                Debug.Log("creating lvl 2 xshots");
                GameObject lowerSubshot = Instantiate(subshot, transform.position, Quaternion.identity);
                lowerSubshot.GetComponent<AudioShot>().xSpeed = xSpeed * .95f;
                lowerSubshot.GetComponent<AudioShot>().ySpeed = -.2f;

                GameObject upperSubshot = Instantiate(subshot, transform.position, Quaternion.identity);
                upperSubshot.GetComponent<AudioShot>().xSpeed = xSpeed * .95f;
                upperSubshot.GetComponent<AudioShot>().ySpeed = .2f;
            }

            if (amplitude > .2)
            {
                Debug.Log("creating lvl 3 xshots");
                GameObject lowerSubshot = Instantiate(subshot, transform.position, Quaternion.identity);
                lowerSubshot.GetComponent<AudioShot>().xSpeed = xSpeed * .90f;
                lowerSubshot.GetComponent<AudioShot>().ySpeed = -.4f;

                GameObject upperSubshot = Instantiate(subshot, transform.position, Quaternion.identity);
                upperSubshot.GetComponent<AudioShot>().xSpeed = xSpeed * .90f;
                upperSubshot.GetComponent<AudioShot>().ySpeed = .4f;
            }

            if (amplitude > .3)
            {
                Debug.Log("creating lvl 4 xshots");
                GameObject lowerSubshot = Instantiate(subshot, transform.position, Quaternion.identity);
                lowerSubshot.GetComponent<AudioShot>().xSpeed = xSpeed * .85f;
                lowerSubshot.GetComponent<AudioShot>().ySpeed = -.6f;

                GameObject upperSubshot = Instantiate(subshot, transform.position, Quaternion.identity);
                upperSubshot.GetComponent<AudioShot>().xSpeed = xSpeed * .85f;
                upperSubshot.GetComponent<AudioShot>().ySpeed = .6f;
            }
        }
    }

    // Update is called once per frame
    void Update () {
        Vector2 velocity = Vector2.zero;

        timeToDeath += Time.deltaTime;
        if ((timeToDeath > 1f) && !alreadyUnpaused) {
            myAudioSizer.Unpause();
            alreadyUnpaused = true;
        }
        if (timeToDeath > lastingTime) {
            Destroy(transform.gameObject);
        }

        velocity.y += ySpeed;
        velocity.x += xSpeed;

        transform.Translate(velocity);
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("In attack collision stay.");
        Debug.Log("target tag is " + other.transform.tag);
        if (other.transform.tag == "Enemy")
        {
            Debug.Log("collide with target");
            other.gameObject.GetComponent<CharacterManager>().LoseHealth(attackDamage);
            myAudioSizer.Unpause();
            Destroy(transform.gameObject);

        }
        if (other.gameObject.name == "SpiderBoss") {
            BossManager myBM = other.GetComponent<BossManager>();
            myBM.BeHurt();
        }
        else if (other.transform.tag == "platform"){
            Debug.Log("But didn't collide with target");
            Destroy(transform.gameObject);

        }
    }
}
