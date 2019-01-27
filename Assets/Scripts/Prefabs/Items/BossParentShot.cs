using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossParentShot : MonoBehaviour {
    private AmplitudeSampler myAmplitudeSampler;
    public int attackDamage = 1;
    public string target = "Enemy";
    public float lastingTime = 5f;
    public float ySpeed = -2f;
    public float xSpeed = 0f;
    public GameObject subshot;

    private float timeToDeath = 0.0f;
    private float amplitude = 0f;

	// Use this for initialization
	void Start () {


        if ((ySpeed > 1f) || (ySpeed < 1f))
        {
            if (true)
            {
                Debug.Log("creating lvl 2 shots");
                //left subshot
                GameObject leftSubshot = Instantiate(subshot, transform.position, Quaternion.identity);
                leftSubshot.GetComponent<BossShot>().xSpeed = -.3f;
                leftSubshot.GetComponent<BossShot>().ySpeed = ySpeed * .95f;

                GameObject rightSubshot = Instantiate(subshot, transform.position, Quaternion.identity);
                rightSubshot.GetComponent<BossShot>().xSpeed = .3f;
                rightSubshot.GetComponent<BossShot>().ySpeed = ySpeed * .95f;
            }

            if (true)
            {
                Debug.Log("creating lvl 3 shots");
                //left subshot
                GameObject leftSubshot = Instantiate(subshot, transform.position, Quaternion.identity);
                leftSubshot.GetComponent<BossShot>().xSpeed = -.6f;
                leftSubshot.GetComponent<BossShot>().ySpeed = ySpeed * .90f;

                GameObject rightSubshot = Instantiate(subshot, transform.position, Quaternion.identity);
                rightSubshot.GetComponent<BossShot>().xSpeed = .6f;
                rightSubshot.GetComponent<BossShot>().ySpeed = ySpeed * .90f;
            }

            if (true)
            {
                Debug.Log("creating lvl 3 shots");
                //left subshot
                GameObject leftSubshot = Instantiate(subshot, transform.position, Quaternion.identity);
                leftSubshot.GetComponent<AudioShot>().xSpeed = -.9f;
                leftSubshot.GetComponent<AudioShot>().ySpeed = ySpeed * .85f;

                GameObject rightSubshot = Instantiate(subshot, transform.position, Quaternion.identity);
                rightSubshot.GetComponent<AudioShot>().xSpeed = .9f;
                rightSubshot.GetComponent<AudioShot>().ySpeed = ySpeed * .85f;
            }
        } else {
            if (amplitude > .1)
            {
                Debug.Log("creating lvl 2 shots");
                GameObject lowerSubshot = Instantiate(subshot, transform.position, Quaternion.identity);
                lowerSubshot.GetComponent<AudioShot>().ySpeed = -.2f;
                lowerSubshot.GetComponent<AudioShot>().xSpeed = xSpeed * .95f;

                GameObject upperSubshot = Instantiate(subshot, transform.position, Quaternion.identity);
                upperSubshot.GetComponent<AudioShot>().ySpeed = .2f;
                upperSubshot.GetComponent<AudioShot>().xSpeed = xSpeed * .95f;
            }

            if (amplitude > .2)
            {
                Debug.Log("creating lvl 3 shots");
                GameObject lowerSubshot = Instantiate(subshot, transform.position, Quaternion.identity);
                lowerSubshot.GetComponent<AudioShot>().ySpeed = -.4f;
                lowerSubshot.GetComponent<AudioShot>().xSpeed = xSpeed * .90f;

                GameObject upperSubshot = Instantiate(subshot, transform.position, Quaternion.identity);
                upperSubshot.GetComponent<AudioShot>().ySpeed = .4f;
                upperSubshot.GetComponent<AudioShot>().xSpeed = xSpeed * .90f;
            }

            if (amplitude > .3)
            {
                Debug.Log("creating lvl 3 shots");
                GameObject lowerSubshot = Instantiate(subshot, transform.position, Quaternion.identity);
                lowerSubshot.GetComponent<AudioShot>().ySpeed = -.6f;
                lowerSubshot.GetComponent<AudioShot>().xSpeed = xSpeed * .85f;

                GameObject upperSubshot = Instantiate(subshot, transform.position, Quaternion.identity);
                upperSubshot.GetComponent<AudioShot>().ySpeed = .6f;
                upperSubshot.GetComponent<AudioShot>().xSpeed = xSpeed * .85f;
            }
        }
    }

    // Update is called once per frame
    void Update () {
        Vector2 velocity = Vector2.zero;

        timeToDeath += Time.deltaTime;
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
        if (other.transform.tag == "Player")
        {
            Debug.Log("collide with target");
            other.gameObject.GetComponent<CharacterManager>().LoseHealth(attackDamage);
            Destroy(transform.gameObject);

        }
        else if (other.transform.tag == "platform"){
            Debug.Log("But didn't collide with target");
            Destroy(transform.gameObject);

        }
    }
}
