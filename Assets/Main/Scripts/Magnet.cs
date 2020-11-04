using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Attraction
{
    public Transform centre;
    public float radius;
    public float force;
}
public class Magnet : MonoBehaviour
{
    public MagnetDistortion distortionEffect;
    [SerializeField] private Attraction attractionField;
    public virtual Attraction AttractionField
    {
        get { return attractionField; }
    }

    public void DrawAttractionField()
    {
        Attraction attraction = AttractionField;
        Gizmos.DrawWireSphere(attraction.centre.position, attraction.radius);
    }
    //[SerializeField] private SphereCollider attractionSphere;

    protected Rigidbody rigidbody;
    

    public virtual void Initialise()
    {
        rigidbody = GetComponent<Rigidbody>();
        //attractionDistance = attractionSphere.radius;
        if (attractionField.centre == null)
        {
            attractionField.centre = transform;
        }

        //attractionSphere.isTrigger = true;
       // Destroy(attractionSphere);
    }

   /* public virtual void UpdateAttractionField()
    {
        Gizmos.dra
    }*/

    public void MovePhysically(Vector3 position)
    {
        rigidbody.MovePosition(position);
    }

    public void RefreshDistortionEffect()
    {
        if(distortionEffect == null)
        {
            Debug.LogError("No Distortion effect on this magnet!");
            return;
        }
        distortionEffect.Refresh(AttractionField.centre, AttractionField.radius);
    }
    /*public virtual void AttachMetalObject(MetalObject metalObject)
    {
        //metalObject.transform.parent = transform;

    }

    public virtual void DetachMetalObject(MetalObject metalObject)
    {
        // metalObject.transform.parent = null;
        //metalObject.joint.connectedBody = null;
       // metalObject.Detach();
    }*/
}
