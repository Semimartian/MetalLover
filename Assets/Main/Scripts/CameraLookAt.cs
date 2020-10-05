using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
    [SerializeField] private Transform camera;
    [SerializeField] private Transform cinemaThing;


    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.position = cinemaThing.position + (camera.forward);

    }
    void Update()
    {
    }
}
