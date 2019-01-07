using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CharacterManager : MonoBehaviour {

    public int health = 1;
    private bool isDead = false;

    protected SpriteRenderer myRenderer;

	// Use this for initialization
	void Start () {
        myRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update () {
        Debug.Log(this.tag + " health is " + health);
        Debug.Log(this.tag + " isDead is " + isDead);
        //check if we're dead
        if ((health <= 0) && !isDead) {
            Die();
        }
	}


    void FlipRenderer() {
        myRenderer.enabled = !myRenderer.enabled;
    }


    public virtual void LoseHealth (int loss) {
        Debug.Log("In charman losehealth");
        health -= loss;
        Debug.Log("Health of " + this.tag + " is now " + health);
    }


    public virtual void GainHealth(int gain) {
        health += gain;
    }

    public void Die() {
        Debug.Log(this.tag + "Should be dying");

        //set animation for death
        isDead = true;
        Destroy(gameObject, .80f);
        InvokeRepeating("FlipRenderer", 0.0f, .08f);

    }
}
