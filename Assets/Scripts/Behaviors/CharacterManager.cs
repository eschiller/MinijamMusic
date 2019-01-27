using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CharacterManager : MonoBehaviour {

    public int health = 3;
    private bool isDead = false;

    protected SpriteRenderer myRenderer;

    public Transform targetTransform;

    // Update is called once per frame
    void Update () {

        //check if we're dead
        if ((health <= 0) && !isDead) {
            Die();
        }
	}


    void FlipRenderer() {
        GetComponent<SpriteRenderer>().enabled = !myRenderer.enabled;
    }


    public virtual void LoseHealth (int loss) {
        Debug.Log("health is " + health);
        health -= loss;
        StartCoroutine("flashRed");
    }


    public virtual void GainHealth(int gain) {
        health += gain;
    }

    public virtual void Die() {

        //set animation for death
        isDead = true;
        Destroy(gameObject, .40f);
        //InvokeRepeating("FlipRenderer", 0.0f, .08f);
        Debug.Log("about to set animation to dead");
        GetComponent<Animator>().SetBool("isDead", true);
    }


    public IEnumerator flashRed() {
        for (int i = 0; i < 4; i++) {
            GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(0.1f);
            GetComponent<SpriteRenderer>().color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void SetTargetTransform(Transform t) {
        targetTransform = t;
    }
}
