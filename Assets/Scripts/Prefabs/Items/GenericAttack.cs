using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericAttack : MonoBehaviour {

    public int attackDamage = 1;
    public string target = "Enemy";
    public float lastingTime = .2f;

    private float timeToDeath = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timeToDeath += Time.deltaTime;
        if (timeToDeath > lastingTime) {
            Destroy(transform.gameObject);
        }
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("In attack collision stay.");
        Debug.Log("target tag is " + other.transform.tag);
        if (other.transform.tag == "Enemy")
        {
            Debug.Log("collide with target");
            other.gameObject.GetComponent<CharacterManager>().LoseHealth(attackDamage);
        } else {
            Debug.Log("But didn't collide with target");
        }
    }
}
