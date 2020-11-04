using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudRing : MonoBehaviour
{
    [SerializeField] private Transform anchor;
    [SerializeField] private float rotationSpeed;
    private Transform myTransform;
    private void Start()
    {
        myTransform = transform;
    }
    private void Update()
    {
        myTransform.position = new Vector3(myTransform.position.x, myTransform.position.y, anchor.position.z);
        myTransform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
    }
}
