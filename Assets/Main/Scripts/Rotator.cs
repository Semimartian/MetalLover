using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Vector3 rotationPerSecond;


    private void FixedUpdate()
    {
       // myTransform.Rotate(rotationPerSecond * Time.deltaTime);

        rigidbody.rotation = Quaternion.Euler(
            rigidbody.rotation.eulerAngles + (rotationPerSecond * Time.deltaTime));
    }
}
