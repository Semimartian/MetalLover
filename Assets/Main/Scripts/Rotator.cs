using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
   private Transform myTransform;
    [SerializeField] private Vector3 rotationPerSecond;
    // Start is called before the first frame update
    void Start()
    {
        myTransform = transform;
    }

    private void FixedUpdate()
    {
        myTransform.Rotate(rotationPerSecond * Time.deltaTime);
    }
}
