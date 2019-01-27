using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossManager : CharacterManager {
    public bool separateEnemies = true;
    public int touchDamange = 1;
    bool isDead = false;

    float hurtTimer = 0f;
    float hurtLength = 1f;
    bool isHurt = false;

    BoxCollider2D myCollider;
    Animator myAnimator;


	// Use this for initialization
	void Start () {
        myCollider = GetComponent<BoxCollider2D>();
        myAnimator = GetComponent<Animator>();
	}

    public override void Die()
    {
        Debug.Log("boss dying");
        //set animation for death
        isDead = true;
        Destroy(gameObject, .40f);
        //InvokeRepeating("FlipRenderer", 0.0f, .08f);
        Debug.Log("about to set animation to dead");
        //GetComponent<Animator>().SetBool("isDead", true);

        GameObject gm = GameObject.Find("GameManager");
    }

    private void Update()
    {
        Debug.Log("boss health is " + health);
        if (isHurt)
        {
            hurtTimer += Time.deltaTime;
            if (hurtTimer > hurtLength)
            {
                myAnimator.SetBool("isHurt", false);
                hurtTimer = 0f;
                isHurt = false;
            }
        }

        if (health < 0) {
            Die();
        }
    }

    public override void LoseHealth(int loss)
    {
        Debug.Log("health is " + health);
        health -= loss;
        StartCoroutine("flashRed");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Colliding enemy!");
        if (other.transform.tag == "Player")
        {
            Debug.Log("colliding with player!");
            other.gameObject.GetComponent<CharacterManager>().LoseHealth(touchDamange);
        }


        if (other.transform.tag == "Enemy")
        {
            //figure out which direction to walk to go towards target X
            CharacterManager charManager = other.gameObject.GetComponent<CharacterManager>();

            if (transform.position.x < charManager.targetTransform.position.x)
            {
                transform.Translate(new Vector3(-5f, 0f, 0f));
            }
            else
            {
                transform.Translate(new Vector3(5f, 0f, 0f));
            }

            if (transform.position.y < charManager.targetTransform.position.y)
            {
                transform.Translate(new Vector3(0f, -5f, 0f));
            }
            else
            {
                transform.Translate(new Vector3(0f, 5f, 0f));
            }
        }
    }

    public void BeHurt() {
        myAnimator.SetBool("isHurt", true);
        isHurt = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
       
    }
}
