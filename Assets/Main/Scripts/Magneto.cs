﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;


public class Magneto : Magnet
{
    [System.Serializable]
    private struct MagnetoLevel
    {
        public float sizeMultiplier;
        public float mass;
        public float speed;

        public Attraction attractionField;
    }

    [SerializeField] private MagnetoLevel[] magnetoLevels;
    private MagnetoLevel magnetoLevel;
    public override Attraction AttractionField
    {
        get { return magnetoLevel.attractionField; }
    }

    [SerializeField] private sbyte currentMagnetoLevelIndex = 0;
    //public sbyte CurrentMagnetoLevelIndex => currentMagnetoLevelIndex;
    private Transform myTransform;

    [SerializeField] float rotationPerSecond;
    [SerializeField] float defaultSpeed;
    [SerializeField] float velocity;
    [SerializeField] Text debugText;
    [SerializeField] private MainCamera camera;

    //[SerializeField] private float currentBoostlessSpeed;

    //[SerializeField] private float speedCheckInterval = 0.3f;
   // [SerializeField] private int attachedObjects;


    public override void Initialise()
    {
        base.Initialise();
        myTransform = transform;

        magnetoLevel = magnetoLevels[0];

        ConformToMagnetoLevel(0,3);
        InvokeRepeating("UpdateDebugText", 1, 0.2f);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;              
    }

    private void ConformToMagnetoLevel(sbyte newLevelIndex,float waitTime)
    {
        if(newLevelIndex >= magnetoLevels.Length)
        {
            Debug.LogError("currentMagnetoLevelIndex>= magnetoLevels.Length!");
            return;
        }
        //MagnetoLevel magnetoLevel = magnetoLevels[currentMagnetoLevelIndex];
        StartCoroutine(Scale(newLevelIndex, waitTime));
    }

    [SerializeField] private Transform body;
    [SerializeField] private Shell shell;


    [SerializeField] private float scaleSpeed;
    private IEnumerator Scale(sbyte newLevelIndex, float waitTime)
    {
        MagnetoLevel nextLevel = magnetoLevels[newLevelIndex];
        float scaleMultiplier = nextLevel.sizeMultiplier;
        float previousMass = rigidbody.mass;
        float previousSize = body.localScale.x;

       // if (currentMagnetoLevelIndex > 0)
        {
            //TODO: this whole logic thing is sloppyy
            float cameraScaler = scaleMultiplier / previousSize;
            camera.ScaleOffsetFromTarget(cameraScaler);
            Debug.Log("CameraScaler" + cameraScaler);
        }

        while (body.localScale.x < scaleMultiplier)
        {
           // Debug.Log( body.localScale.x + "<"+ multiplier);

            yield return new WaitForFixedUpdate();
            Vector3 scaleAddition = Vector3.one * (scaleMultiplier * Time.fixedDeltaTime * scaleSpeed);
            body.localScale += scaleAddition;
            shell.transform.localScale += scaleAddition;
            {
                float newMass = Mathf.Lerp(previousMass, nextLevel.mass,
                  ((body.localScale.x - previousSize) / (scaleMultiplier - previousSize)));
                rigidbody.mass = newMass;
            }
        }
        body.localScale = Vector3.one * scaleMultiplier;
        shell.transform.localScale = Vector3.one * scaleMultiplier;

        centreOfMassTransform.localPosition = centreOfMassTransform.localPosition * (scaleMultiplier / previousSize);

        rigidbody.mass = nextLevel.mass;

        magnetoLevel = nextLevel;

        MagnetManager.ConformToMagnetoLevel(newLevelIndex, waitTime);
        currentMagnetoLevelIndex = newLevelIndex;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            currentMagnetoLevelIndex++;
            ConformToMagnetoLevel((sbyte)(currentMagnetoLevelIndex + 1),0);
        }
    }


    private static readonly float smallXRotaionClampValue = 60;
    private static readonly float largeXRotaionClampValue = 360 - smallXRotaionClampValue;
    [SerializeField]
    private Transform centreOfMassTransform ;
    private Vector3 centreOfMass;
    private void FixedUpdate()
    {
         velocity = rigidbody.velocity.magnitude;
  
        //if (!attached)
        float mouseMovement = Input.GetAxisRaw("Mouse X");
        float deltaTime = Time.fixedDeltaTime;

        Quaternion currentRotationQurternion = rigidbody.rotation;
        if (mouseMovement != 0)
        {
            /*myTransform.Rotate(new Vector3
                (0, rotationPerSecond * mouseMovement * deltaTime, 0));*/

            Vector3 currentRotation = currentRotationQurternion.eulerAngles;
            currentRotation += new Vector3
                (0, rotationPerSecond * mouseMovement * deltaTime, 0);
            currentRotationQurternion = Quaternion.Euler(currentRotation);
             rigidbody.rotation = currentRotationQurternion;
        }

        Vector3 Movement = new Vector3();
        if (Input.GetMouseButton(0))
        {
            float speed = 
                ( magnetoLevel.speed + speedBoost);// * rigidbody.mass;//TODO: casche
            Movement = myTransform.forward * speed * deltaTime;
            rigidbody.AddForce
                (Movement, ForceMode.VelocityChange);

            //myTransform.Translate(Vector3.forward * currentSpeed * deltaTime);
        }
        //Debug.Log("-----------");


       // Quaternion currentRotationQurternion = rigidbody.rotation;
       // Vector3 currentRotation = currentRotationQurternion.eulerAngles;// myTransform.eulerAngles;
                                                                        //Debug.Log("angle x" + currentRotation.x);
                                                                        //currentRotation.x = Mathf.Clamp(currentRotation.x, clampValue, 360 - clampValue);

        //rigidbody.angularVelocity = new Vector3()
        // Debug.Log("angularVelocity" + angularVelocity);
       /* Debug.Log("rigidbody.centerOfMass" + rigidbody.centerOfMass);

        rigidbody.centerOfMass =  centreOfMassTransform.localPosition;*/

        if (false)
        {
            /*if (currentRotation.x > smallXRotaionClampValue && currentRotation.x < largeXRotaionClampValue)
            {
                Vector3 angularVelocity = rigidbody.angularVelocity;
                rigidbody.angularVelocity = new Vector3(0, angularVelocity.y, angularVelocity.z);
                Debug.Log("angle x" + currentRotation.x);*/
                /*currentRotation.x = smallXRotaionClampValue;
                currentRotationQurternion = Quaternion.Euler(currentRotation);*/
                // rigidbody.MoveRotation(currentRotationQurternion);

           // }
            //Debug.Log("angle x" + currentRotation.x);
        }



        /* float time = Time.time;
         if (time > lastSpeedCheck + speedCheckInterval)
         {
             UpdateSpeed();
             lastSpeedCheck = time;
         }*/

        shell.rigidbody.MovePosition(rigidbody.position + (Movement*0.1f));
        shell.rigidbody.MoveRotation(currentRotationQurternion);
    }

  
    public void AddForce(Vector3 force, ForceMode mode)
    {
        rigidbody.AddForce(force, mode);
    }

    
   [SerializeField] private float speedBoost;

    private void OnTriggerEnter(Collider other)
    {
        SpeedBooster speedBooster = other.GetComponent<SpeedBooster>();
        if (speedBooster != null)
        {
            speedBoost = speedBooster.SpeedBoost;
        }
        else
        {
            Portal portal = other.GetComponent<Portal>();
            if (portal != null && portal.IsActive)
            {
                if(currentMagnetoLevelIndex < magnetoLevels.Length - 1)
                {
                    Debug.Log("portal" + Time.frameCount);
                    portal.Expire();
                    ConformToMagnetoLevel((sbyte)(currentMagnetoLevelIndex + 1),0);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        SpeedBooster speedBooster = other.GetComponent<SpeedBooster>();
        if (speedBooster != null)
        {
            speedBoost = 0;
        }
    }


    private void UpdateDebugText()
    {
        debugText.text = "Velocity: " + velocity.ToString("f3") + "\n"/* +
        "Attached: " + attachedObjects*/;
    }
    //public float GetVelocity() => velocity;

}
