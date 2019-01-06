using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Transform ownerObject;

    PlatformerController2D pControl;


    // Use this for initialization
    void Start()
    {
        pControl = GetComponent<PlatformerController2D>();
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
        pControl.disable();
        ownerObject = newOwner;
    }

    public void throwItem(Vector2 direction) {
        Debug.Log("throwing item");
        ownerObject = null;
        pControl.enable();
        pControl.setActiveXVel(2.0f * direction.x);
    }
}
