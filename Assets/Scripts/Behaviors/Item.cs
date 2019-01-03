using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Transform ownerObject;

    PlatformerPhysics pphys;


    // Use this for initialization
    void Start()
    {
        pphys = GetComponent<PlatformerPhysics>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ownerObject)
        {
            transform.position = ownerObject.position;
        }
    }

    public void grabItem(Transform newOwner) {
        pphys.disable();
        ownerObject = newOwner;
    }

    public void throwItem(Vector2 direction) {
        Debug.Log("throwing item");
        ownerObject = null;
        pphys.enable();
        pphys.setXVel(2.0f * direction.x);
    }
}

