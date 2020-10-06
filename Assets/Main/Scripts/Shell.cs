using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Rigidbody rigidbody;

    private void FixedUpdate()
    {
        rigidbody.MovePosition(target.position);
        rigidbody.MoveRotation(target.rotation);
    }

}