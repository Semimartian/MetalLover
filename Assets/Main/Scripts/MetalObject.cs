using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalObject : MonoBehaviour
{
    //public bool isAttached;
    public Rigidbody rigidbody;
    public float attractionForce;
    [SerializeField] private sbyte tier;

    public bool permenantlyAttatched = false;
    public sbyte Tier
    {
        get { return tier; }
    }
    //public Magnet magnetAttached =null;
    /*public bool IsAttachedTo(Magnet magnet)
    {
        return magnetAttached == magnet;
    }

    public bool IsAttachedToSomeMagnet()
    {
        return magnetAttached != null;
    }*/


    //public float attraction;
    /*private void AttachTo(Magnet magnet, Rigidbody body)
    {
        return;
        magnetAttached = magnet;
        //rigidbody.isKinematic = true;
        magnet.AttachMetalObject(this);

        Joint joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = body;
    }
    */
    /*public void Detach()
    {
        return;

        magnetAttached.DetachMetalObject(this);
        Destroy(gameObject.GetComponent<Joint>());

        magnetAttached = null;
        rigidbody.isKinematic = false;
    }*/
}
