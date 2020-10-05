using System.Collections;
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
        public Attraction attractionField;
    }

    [SerializeField] private MagnetoLevel[] magnetoLevels;
    private MagnetoLevel magnetoLevel;
    public override Attraction AttractionField
    {
        get { return magnetoLevel.attractionField; }
    }

    [SerializeField] private int currentMagnetoLevelIndex = 0;

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
        ConformToMagnetoLevel();
        InvokeRepeating("UpdateDebugText", 1, 0.2f);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;              
    }

    private void ConformToMagnetoLevel()
    {
        magnetoLevel = magnetoLevels[currentMagnetoLevelIndex];
        if(currentMagnetoLevelIndex>= magnetoLevels.Length)
        {
            Debug.LogError("currentMagnetoLevelIndex>= magnetoLevels.Length!");
            return;
        }
        //MagnetoLevel magnetoLevel = magnetoLevels[currentMagnetoLevelIndex];
        StartCoroutine(Scale());

    }

    [SerializeField] private Transform body;
    [SerializeField] private float scaleSpeed;
    private IEnumerator Scale()
    {
        if (currentMagnetoLevelIndex > 0)
        {
            //TODO: this whole logic thing is sloppyy
            camera.ScaleOffsetFromTarget(1.5f);

        }
        float scaleMultiplier = magnetoLevel.sizeMultiplier;
        float previousMass = rigidbody.mass;
        float previousSize = body.localScale.x;

        while (body.localScale.x < scaleMultiplier)
        {
           // Debug.Log( body.localScale.x + "<"+ multiplier);

            yield return new WaitForFixedUpdate();
            body.localScale += Vector3.one * (scaleMultiplier * Time.fixedDeltaTime * scaleSpeed);

            {
                float newMass = Mathf.Lerp(previousMass, magnetoLevel.mass,
                  ((body.localScale.x - previousSize) / (scaleMultiplier - previousSize)));
                rigidbody.mass = newMass;
            }
        }
        body.localScale = Vector3.one * scaleMultiplier;
        rigidbody.mass = magnetoLevel.mass;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            currentMagnetoLevelIndex++;
            ConformToMagnetoLevel();
        }
    }

    private void FixedUpdate()
    {
         velocity = rigidbody.velocity.magnitude;
  
        //if (!attached)
        {
            float mouseMovement = Input.GetAxisRaw("Mouse X");
            float deltaTime = Time.fixedDeltaTime;
            if (mouseMovement != 0)
            {
                // Debug.Log("mouseMovement>0");
                myTransform.Rotate(new Vector3
                    (0, rotationPerSecond * mouseMovement * deltaTime, 0));
            }
            if (Input.GetMouseButton(0))
            {
                float speed = (defaultSpeed + speedBoost);// * rigidbody.mass;//TODO: casche
                rigidbody.AddForce
                    (myTransform.forward * speed , ForceMode.Acceleration);
                //myTransform.Translate(Vector3.forward * currentSpeed * deltaTime);
            }

        }

       /* float time = Time.time;
        if (time > lastSpeedCheck + speedCheckInterval)
        {
            UpdateSpeed();
            lastSpeedCheck = time;
        }*/
    }
    private List<MetalObject> metalObjectsTouching = new List<MetalObject>();
    private void OnCollisionEnter(Collision collision)
    {
        MetalObject metalObject = collision.gameObject.GetComponent<MetalObject>();
        if (metalObject != null)
        {
            if (!metalObjectsTouching.Contains(metalObject))
            {
                metalObjectsTouching.Add(metalObject);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        MetalObject metalObject = collision.gameObject.GetComponent<MetalObject>();
        if (metalObject != null)
        {
            metalObjectsTouching.Remove(metalObject);
        }
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
                    currentMagnetoLevelIndex++;
                    ConformToMagnetoLevel();
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
