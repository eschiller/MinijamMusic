/*
 * CharacterManager class is intended to house functionality shared across
 * all characters, such asPlayers, Enemies, NPCs, etc... possibly even some 
 * entities such as breakable walls. 
 */
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

        //check if we're dead
        if ((health <= 0) && !isDead) {
            Die();
        }
	}


    void FlipRenderer() {
        myRenderer.enabled = !myRenderer.enabled;
    }


    public virtual void LoseHealth (int loss) {
        health -= loss;
    }


    public virtual void GainHealth(int gain) {
        health += gain;
    }

    /*
     * Destroys the game object and blinks it for a specified amount of time
     */
    public void Die(float deathDelay = 0.8f, float blinkDelay=.08f) {
        //set animation for death
        isDead = true;
        Destroy(gameObject, deathDelay);
        InvokeRepeating("FlipRenderer", 0.0f, blinkDelay);

    }
}
