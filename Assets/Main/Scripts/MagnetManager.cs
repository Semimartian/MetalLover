using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;
using System;

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
    [SerializeField] private float metalObjectsDrag;
    [SerializeField] private float metalObjectsAngularDrag;
    [SerializeField] private MetalObjectProperties[] metalObjectPropertiesByTiers;
    private MetalObject[] metalObjects;
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

        Debug.Log(Vector3.one.magnitude);
        Debug.Log((Vector3.one * 2).magnitude);

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
            metalObject.rigidbody.drag = metalObjectsDrag;
            metalObject.rigidbody.angularDrag = metalObjectsAngularDrag;
            //metalObject.attractionForce = properties.attraction;
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
       // ManageMagnetoAgainstMagnets();

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

            Gizmos.DrawWireSphere
               (magnetAttraction.centre.position, (magnetAttraction.radius / permenantAttachmentRadiusDevider));
        }

        
    }
    #endregion
    public static void ConformToMagnetoLevel(sbyte level, float waitTime)
    {
        MagnetoLevelIndex = level;
        instance.Invoke("MyConformToMagnetoLevel", waitTime);
    }

    private static sbyte MagnetoLevelIndex;
    [SerializeField] private int higherTierLayer;
    [SerializeField] private int currentTierLayer;


    private void MyConformToMagnetoLevel()
    {
        for (int i = 0; i < metalObjects.Length; i++)
        {
            MetalObject metalObject = metalObjects[i];
            if (!metalObject.permenantlyAttatched)
            {
                int outlineIndex;
                int physicsLayer;

                if (metalObject.Tier <= MagnetoLevelIndex)
                {
                    if (metalObject.rigidbody != null)
                    {
                        metalObject.rigidbody.isKinematic = false;
                        metalObject.rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;

                    }
                    outlineIndex = 0;
                    physicsLayer = currentTierLayer;
                }
                else
                {
                    if (metalObject.rigidbody != null)
                    {
                        metalObject.rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                        metalObject.rigidbody.isKinematic = true;

                    }
                    outlineIndex = 1;
                    physicsLayer = higherTierLayer;
                }

                ModifyMetalObject(metalObject.transform, outlineIndex, physicsLayer);
            }      
        }
    }

    /*private void InfiniteLoop()
    {
        InfiniteLoop();
    }*/

    private static void ModifyPhysicsLayerOf(Transform t,  int physicsLayer)
    {
        t.gameObject.layer = physicsLayer;    

        for (int j = 0; j < t.childCount; j++)
        {
            Transform child = t.GetChild(j);
            if (child.gameObject.activeSelf)
            {
                ModifyPhysicsLayerOf(child, physicsLayer);
            }
        }
    }

    private void ModifyMetalObject(Transform t, int outlineIndex, int physicsLayer )
    {
        t.gameObject.layer = physicsLayer;
        MeshRenderer meshRenderer = t.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            Outline outline = t.GetComponent<Outline>();
            if (outline == null)
            {
                outline = t.gameObject.AddComponent<Outline>();
            }
            outline.color = outlineIndex;
        }

        for (int j = 0; j < t.childCount; j++)
        {
            Transform child = t.GetChild(j);
            if (child.gameObject.activeSelf)
            {
                ModifyMetalObject(child, outlineIndex, physicsLayer);
            }          
        }
    }

    private void RemoveCollidersFrom(Transform t)
    {
        Collider[] colliders = t.GetComponents<Collider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            Destroy(colliders[i]);
        }

        for (int j = 0; j < t.childCount; j++)
        {
            Transform child = t.GetChild(j);
            if (child.gameObject.activeSelf)
            {
                RemoveCollidersFrom(child);
            }
        }
    }

    private float boundsExpansion = 3f;
    private float permenantAttachmentRadiusDevider = 15;
    private void ManageMetalObjects(/*float deltaTime*/)
    {
        sbyte magnetoLevel = MagnetoLevelIndex;
        for (int j = 0; j < magnets.Length; j++)
        {
            bool isMagneto = /*metalObjectsToAttractMagneto &&*/( magnets[j] == magneto);
            Attraction magnetAttraction = magnets[j].AttractionField;
            Bounds bounds = new Bounds
                (magnetAttraction.centre.position, Vector3.one * magnetAttraction.radius * boundsExpansion);
            float magnetAttractionDistance = magnetAttraction.radius;

            // Vector3 magnetPosition = magnets[j].attrractivePoint.position;
            for (int i = 0; i < metalObjects.Length; i++)
            {
                MetalObject metalObject = metalObjects[i];
                if (metalObject.permenantlyAttatched || metalObject.Tier > magnetoLevel)
                {
                    continue;
                }
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
                        #region old but might come back:
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



                        // bool attract = true;
                        //if ( false && isMagneto)// && magnetoLevel < metalObject.Tier)
                        {
                            /* if( false && magnetoLevel < metalObject.Tier)
                             {
                                 if (metalObjectsToAttractMagneto )
                                 {
                                     magneto.AddForce
                                     ((metalObjectPosition - magnetAttraction.centre.position).normalized
                                     * (metalObject.attractionForce /** deltaTime), ForceMode.Force);
                                 }
                             }
                             else *//*if (magnetoLevel > metalObject.Tier)
                             {
                                 if(distanceFromMagnet < magnetAttraction.radius / permenantAttachmentRadiusDevider)
                                 {
                                     AttatchPermenantly(metalObject,magneto);
                                     attract = false;
                                 }
                             }*/
                        }

                        // if (attract)
                        #endregion

                        {
                            metalObject.rigidbody.AddForce
                              ((magnetAttraction.centre.position - metalObjectPosition).normalized
                              * (attractionSpeed /** deltaTime*/), ForceMode.Force);


                        }
                  }
                }
            }
        }     
    }

    private const int MAGNETO_LAYER = 8;
    private const int MAGNETO_SHELL_LAYER = 9;
    public static void AttachToMagnetoShell(MetalObject metalObject, Vector3 point)
    {
        Transform metalObjectTransform = metalObject.transform;
        Rigidbody metalObjectRigidbody =  metalObject.rigidbody;
        metalObjectRigidbody.velocity = Vector3.zero;
        Destroy(metalObjectRigidbody);

        metalObjectTransform.position = point;// metalObjectRigidbody.position;

        ModifyPhysicsLayerOf(metalObjectTransform, MAGNETO_SHELL_LAYER);

        metalObjectTransform.parent = instance.magnetoShellTransform;

        metalObject.permenantlyAttatched = true;
    }

    private void AttatchPermenantly(MetalObject metalObject, Magnet magnet)
    {
        metalObject.rigidbody.velocity = Vector3.zero;
        metalObject.permenantlyAttatched = true;
        Destroy(metalObject.rigidbody);
        RemoveCollidersFrom(metalObject.transform);

        metalObject.transform.parent = magnet.transform;
    }

    [SerializeField] private Transform magnetoShellTransform;
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
