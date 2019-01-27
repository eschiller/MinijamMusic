using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyManager : CharacterManager {
    public bool separateEnemies = true;
    public int touchDamange = 1;

    BoxCollider2D myCollider;


	// Use this for initialization
	void Start () {
        myCollider = GetComponent<BoxCollider2D>();
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

    private void OnTriggerStay2D(Collider2D collision)
    {
       
    }
}
