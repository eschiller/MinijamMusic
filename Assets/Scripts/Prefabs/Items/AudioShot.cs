using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioShot : MonoBehaviour {
    private AmplitudeSampler myAmplitudeSampler;
    public int attackDamage = 1;
    public string target = "Enemy";
    public float lastingTime = 5f;
    public float ySpeed = 1f;
    public float xSpeed = 0f;

    private float timeToDeath = 0.0f;
    private float amplitude = 0f;

	// Use this for initialization
	void Start () {

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
        if (other.transform.tag == "Enemy")
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
