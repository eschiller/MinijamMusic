using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerManager : CharacterManager
{
    GameManager myGameMgr;
    HUDManager myHUDMgr;

    // Use this for initialization
    void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        myGameMgr = GameObject.Find("GameManager").GetComponent<GameManager>();
        myHUDMgr = myGameMgr.getHUDManager();
        myHUDMgr.SetHealth(this.health);
    }


    public override void LoseHealth(int loss)
    {
        Debug.Log("In player manager losehealth");
        health -= loss;
        myHUDMgr.SetHealth(this.health);
    }


    public override void GainHealth(int gain)
    {
        health += gain;
        myHUDMgr.SetHealth(this.health);
    }
}