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

    public void Refresh(Transform target,float radius)
    {
        float normalRadius = 2;
        this.target = target;
        ParticleSystem.ShapeModule module =  particleSystem.shape;

        module.radius = normalRadius;
        this.transform.localScale = Vector3.one * (radius / normalRadius);
       // 

       // particleSystem.set
        //particleSystem.shape = new ParticleSystem.ShapeModule();
    }

}
