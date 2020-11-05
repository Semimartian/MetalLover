using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Attraction
{
    public Transform attractopnPoint;
    public float radius;
    public float force;
}
public class Magnet : MonoBehaviour
{
    public MagnetDistortion distortionEffect;
    public Transform distortionEffectPoint;

    [SerializeField] private Attraction attractionField;
    public virtual Attraction AttractionField
    { 
        get { return attractionField; }
    }

    public void DrawAttractionField()
    {
        Attraction attraction = AttractionField;
        Gizmos.DrawWireSphere(attraction.attractopnPoint.position, attraction.radius);
    }
    //[SerializeField] private SphereCollider attractionSphere;

    protected Rigidbody rigidbody;
    

    public virtual void Initialise()
    {
        rigidbody = GetComponent<Rigidbody>();
        //attractionDistance = attractionSphere.radius;
        if (attractionField.attractopnPoint == null)
        {
            attractionField.attractopnPoint = transform;
        }

        //attractionSphere.isTrigger = true;
       // Destroy(attractionSphere);
    }

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
        distortionEffect.Refresh(distortionEffectPoint, AttractionField.radius);
       // distortionEffect.Refresh(AttractionField.attractopnPoint, AttractionField.radius);

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
