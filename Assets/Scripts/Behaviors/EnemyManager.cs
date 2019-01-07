﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : CharacterManager {
    public int touchDamange = 1;

    BoxCollider2D myCollider;


	// Use this for initialization
	void Start () {
        myCollider = GetComponent<BoxCollider2D>();
	}


    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("In on collision stay.");
        if (other.transform.tag == "Player") {
            other.gameObject.GetComponent<CharacterManager>().LoseHealth(touchDamange);
        }
    }
}
