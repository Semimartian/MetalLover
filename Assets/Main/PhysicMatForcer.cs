using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicMatForcer : MonoBehaviour
{
    [SerializeField] PhysicMaterial physicMaterial;

    void Start()
    {
        Force();
    }

    private void Force()
    {
        Modify(transform);

    }

    private void Modify(Transform t)
    {
        Collider collider = t.GetComponent<Collider>();
        if (collider != null)
        {
            collider.material = physicMaterial;
        }

        for (int i = 0; i < t.childCount; i++)
        {
            Transform child = t.GetChild(i);
            if (child.gameObject.activeSelf)
            {
                Modify(child);
            }
        }
    }


    

    private void MeFunc()
    {
        bool someBool = true;
        if (someBool)
        {
            //Lets try type something
         
           // We are releaning how to type....
          //Am I typing well???
          //אני לומד להקליד עכשיו....
          //..... אמממ מעניין כמה שגיאות אני עושה.....
          // מה הוא גובה הישיבה הנכון????
                     


        }
    }
}
