using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalObject : MonoBehaviour
{
    private const int MAGNETO_LAYER = 8;
    private const int MAGNETO_SHELL_LAYER = 9;
    //public bool isAttached;
    public Rigidbody rigidbody;
   // public float attractionForce;

    [SerializeField] private sbyte tier;

    public bool permenantlyAttatched = false;
    public sbyte Tier
    {
        get { return tier; }
    }

    private void OnCollisionEnter(Collision collision)
    {
        int collisionLayer = collision.gameObject.layer;
        if (collisionLayer == MAGNETO_SHELL_LAYER && collision.collider.gameObject.name == "Magnet")
        {
            SoundNames soundName;
            switch (tier)
            {
                case 0:
                    soundName = SoundNames.Tier0Impact;break;
                case 1:
                    soundName = SoundNames.Tier1Impact; break;
                case 2:
                    soundName = SoundNames.Tier2Impact; break;
                default:
                    soundName = SoundNames.Tier0Impact; break;

            }

            int contactCount = collision.contactCount;
            Debug.Log("collision.contactCount: " + contactCount);

            /* Vector3 averagePosition = new Vector3();// rigidbody.position* contactCount;
             for (int i = 0; i < contactCount; i++)
             {
                 averagePosition += collision.GetContact(i).point;
             }
             averagePosition /= (contactCount);//*2);*/
            var contacts =  collision.contacts;
           // float distance = 0;
            Vector3 averagePoint = new Vector3();
            //Vector3 point = new Vector3();
            for (int i = 0; i < contactCount; i++)
            {
               // distance += contacts[i].separation;
                averagePoint += contacts[i].point;
                //averagePoint = contacts[i].thisCollider.
                Debug.Log("separation" + contacts[i].separation);
            }


            averagePoint /= contactCount;
            float distance = (rigidbody.position - collision.rigidbody.position).magnitude /5;
            /* Vector3 seperation =
                 (contacts[0].otherCollider.ClosestPoint(averagePoint) 
                 - contacts[0].thisCollider.ClosestPoint(averagePoint));*/
            // distance /= contactCount;
            Vector3 point = (rigidbody.position + (averagePoint * distance)) / (distance+1);
                // seperation;
              //  (distance * (this.rigidbody.position-collision.rigidbody.position ).normalized);


            SoundManager.PlayOneShotSoundAt(soundName, point);

            MagnetManager.AttachToMagnetoShell(this, point);

        }
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
