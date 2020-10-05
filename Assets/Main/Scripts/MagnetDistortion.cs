using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetDistortion : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private ParticleSystem particleSystem;

    private void FixedUpdate()
    {
        transform.position = target.position;
    }

    public void Constructor(Transform target,float radius)
    {
        this.target = target;
        ParticleSystem.ShapeModule module =  particleSystem.shape;
       
        module.radius = radius;

       // particleSystem.set
        //particleSystem.shape = new ParticleSystem.ShapeModule();
    }
}
