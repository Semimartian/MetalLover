using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetManager : MonoBehaviour
{

    private static MagnetManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

        }
        else
        {
            Debug.LogError("Can't have more than one MagnetManager!");
        }
    }

    [System.Serializable]
    private struct MetalObjectProperties
    {
        public float attraction;
        public float mass;
    }

    [SerializeField] private MetalObjectProperties[] metalObjectPropertiesByTiers;
    private MetalObject[] metalObjects;
    //[SerializeField] private Transform magnet;
    private Magnet[] magnets;
    [SerializeField] private MagnetDistortion MagnetDistortionPreFab;

    [SerializeField]
    private bool metalObjectsToAttractMagneto;


    void Start()
    {
        metalObjects = FindObjectsOfType<MetalObject>();
        magnets = FindObjectsOfType<Magnet>();
        for (int i = 0; i < magnets.Length; i++)
        {
            magnets[i].Initialise();
        }
        CreateMagnetDistortions();
        InitailaiseMetalObjects();
    }

    private void CreateMagnetDistortions()
    {
        for (int i = 0; i < magnets.Length; i++)
        {
            MagnetDistortion distortion = Instantiate(MagnetDistortionPreFab);
            distortion.Constructor(magnets[i].AttractionField.centre, magnets[i].AttractionField.radius);
        }
    }

    [SerializeField] private PhysicMaterial metalPhysicsMat;
    private void InitailaiseMetalObjects()
    {
        for (int i = 0; i < metalObjects.Length; i++)
        {
            MetalObject metalObject = metalObjects[i];
            metalObject.transform.localScale = Vector3.one;
            ref MetalObjectProperties properties =ref metalObjectPropertiesByTiers[metalObject.Tier];
            metalObject.rigidbody.mass = properties.mass;
            metalObject.attractionForce = properties.attraction;
           Collider[] colliders= metalObject.GetComponentsInChildren<Collider>();
            for (int j = 0; j < colliders.Length; j++)
            {
                colliders[j].material = metalPhysicsMat;
            }
        }
    }

    private void FixedUpdate()
    {
       // float deltaTime = Time.deltaTime;
        ManageMetalObjects();
        ManageMagnetoAgainstMagnets();

    }

    #region Draw:
    private bool drawAttractionFields = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            drawAttractionFields = !drawAttractionFields;
        }   
    }

    private void OnDrawGizmos()
    {
        if (drawAttractionFields)
        {
            for (int j = 0; j < magnets.Length; j++)
            {
                magnets[j].DrawAttractionField();
            }

            Attraction magnetAttraction = magneto.AttractionField;

            Gizmos.DrawWireCube
                (magnetAttraction.centre.position, Vector3.one * magnetAttraction.radius * boundsExpansion);
        }

        
    }
    #endregion
    public static void ConformToMagnetoLevel(sbyte level)
    {
        instance.MyConformToMagnetoLevel(level);
    }

    [SerializeField] private int currentTierLayer;
    private void MyConformToMagnetoLevel(sbyte level)
    {
        for (int i = 0; i < metalObjects.Length; i++)
        {
            MetalObject metalObject = metalObjects[i];
            if(metalObject.Tier <= level)
            {
                metalObject.gameObject.layer = currentTierLayer;
                int childCount = metalObject.transform.childCount;
                for (int j = 0; j < childCount; j++)
                {
                    metalObject.transform.GetChild(j).gameObject.layer = currentTierLayer;
                }
            }
        }
    }

    private float boundsExpansion = 3f;
    private void ManageMetalObjects(/*float deltaTime*/)
    {
        sbyte magnetoLevel = magneto.CurrentMagnetoLevelIndex;
        for (int j = 0; j < magnets.Length; j++)
        {
            bool isMagneto = metalObjectsToAttractMagneto &&( magnets[j] == magneto);
            Attraction magnetAttraction = magnets[j].AttractionField;
            Bounds bounds = new Bounds
                (magnetAttraction.centre.position, Vector3.one * magnetAttraction.radius * boundsExpansion);
            float magnetAttractionDistance = magnetAttraction.radius;

            // Vector3 magnetPosition = magnets[j].attrractivePoint.position;
            for (int i = 0; i < metalObjects.Length; i++)
            {
                MetalObject metalObject = metalObjects[i];
                //if (!metalObject.IsAttachedTo(magnets[j]))
                {


                        Transform metalObjectTransform = metalObject.transform;
                    Vector3 metalObjectPosition = metalObjectTransform.position;
                    if (!bounds.Contains(metalObjectPosition))
                    {
                        continue;
                    }
                    float distanceFromMagnet = 
                        Vector3.Distance(magnetAttraction.centre.position, metalObjectPosition);
                  if (distanceFromMagnet <= magnetAttractionDistance)
                  {
                       // Debug.Log("distanceFromMagnet" + distanceFromMagnet);
                        float attractionSpeed =
                          (Mathf.Abs(distanceFromMagnet - magnetAttractionDistance) / magnetAttractionDistance) 
                          * magnetAttraction.force;
                        /*if (metalObject.IsAttachedToSomeMagnet())
                        {
                            if(attractionSpeed> metalObject.attraction)
                            {
                                metalObject.Detach();
                            }
                            else
                            {
                                continue;
                            }
                        }*/

                        metalObject.rigidbody.AddForce
                          ((magnetAttraction.centre.position - metalObjectPosition).normalized
                          * (attractionSpeed /** deltaTime*/),ForceMode.Force);
                        if (isMagneto && magnetoLevel < metalObject.Tier)
                        {
                            magneto.AddForce
                                ((metalObjectPosition - magnetAttraction.centre.position ).normalized
                                * (metalObject.attractionForce /** deltaTime*/), ForceMode.Force);
                        }
                  }
                }
            }
        }     
    }


    [SerializeField] private Magneto magneto;
    [SerializeField] private float MagnetsForceAgainstMagnetoMultiplier=3f;

    private void ManageMagnetoAgainstMagnets(/*float deltaTime*/)
    {
        Vector3 magnetoPosition = magneto.transform.position;

        for (int j = 0; j < magnets.Length; j++)
        {
            if(magnets[j] == magneto)
            {
                continue;//O4TODO:Optimise
            }
            Attraction magnetAttraction = magnets[j].AttractionField;
           // Vector3 magnetPosition = magnets[j].attrractivePoint.position;
            //Transform metalObjectTransform = metalObject.transform;
            float distanceFromMagnet = Vector3.Distance(magnetAttraction.centre.position, magnetoPosition);
            float magnetAttractionDistance = magnetAttraction.radius;
            if (distanceFromMagnet <= magnetAttractionDistance)
            {
                /* if (metalObject.IsAttachedToSomeMagnet())
                 {
                     metalObject.Detach();
                 }*/
                float attractionSpeed =
                    (Mathf.Abs(distanceFromMagnet - magnetAttractionDistance) / magnetAttractionDistance)
                    * magnetAttraction.force * MagnetsForceAgainstMagnetoMultiplier;

                magneto.AddForce
                    ((magnetAttraction.centre.position - magneto.transform.position).normalized
                    * (attractionSpeed /** deltaTime*/), ForceMode.Force);
            }       
        }
    }

}
