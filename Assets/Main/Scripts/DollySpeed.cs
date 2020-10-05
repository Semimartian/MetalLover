using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollySpeed : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
   [SerializeField] private CinemachineDollyCart cart;
   

    private CinemachineSmoothPath path;
    
    // [SerializeField] private Magneto _controller;

    private void Start()
    {
        // _controller = FindObjectOfType<Magneto>();
        previousCameraPosition = virtualCamera.transform.position;
    }
    private Vector3 previousCameraPosition;

    private void FixedUpdate()
    {
        //float pos = virtualCamera.

        // cart.m_Position = 2;
        //  transform.position = path.EvaluatePosition()
        Vector3 cameraPosition = virtualCamera.transform.position;
        cart.m_Position += Vector3.Distance(cameraPosition,previousCameraPosition);

        previousCameraPosition = cameraPosition;
    }
}
