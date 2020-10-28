using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//WARNIN':UGLIEST CODE IN THE ENTIRE PROJECT:
public class WarpDriveEffect : MonoBehaviour
{
    //private Transform myTransform;
    [SerializeField] private Transform target;
    private float targetPrevZ;
    //[SerializeField] private float ZOffset;
    [SerializeField] private ParticleSystem particleSystem;

    [SerializeField] private float emissionRate;
    [SerializeField] private Material particlesMat;


    private void FixedUpdate()
    {
        float deltaTime = Time.fixedDeltaTime;
        float targetZ = target.position.z;

        float difference = (targetZ - targetPrevZ) * deltaTime;
        float newEmissionRate = difference * emissionRate;
        float currentEmmitionRate = particleSystem.emissionRate;
       // Debug.Log(difference/ Time.fixedDeltaTime);
       
        if (  Input.GetMouseButton(0) && difference / deltaTime > 0.06f)
        {
            desiredAlpha = 1;
            // StartCoroutine(FadeIn());

        }
        else 
        {
            // StartCoroutine(FadeOut());
            desiredAlpha = 0;
        }
        particleSystem.emissionRate = newEmissionRate;
        targetPrevZ = targetZ;


        float alpha = particlesMat.color.a;
        if(alpha != desiredAlpha)
        {
            float multiplier = alpha > desiredAlpha ? -1 : 1;
            Debug.Log("fading");
            Debug.Log("alpha" + alpha);

            alpha += fadeSpeed * deltaTime * multiplier;
            Color currentColour = particlesMat.color;
            currentColour.a = Mathf.Clamp( alpha,0,1);
            particlesMat.color = currentColour;
        }
    }

    float desiredAlpha;
    float fadeSpeed = 2.5f;
    /*private IEnumerator FadeOut()
    {
        Debug.Log("FadeEvent");

        float alpha = particlesMat.color.a;
       //// Debug.Log("alpha" + alpha);
        while (alpha > 0)
        {
            alpha -= fadeSpeed * Time.deltaTime;
            Color currentColour = particlesMat.color;
            currentColour.a = alpha;
            particlesMat.color = currentColour;
            yield return null;
        }
    }

    private IEnumerator FadeIn()
    {
        Debug.Log("FadeEvent" );
        float alpha = particlesMat.color.a;
       // Debug.Log("alpha" + alpha);
        while (alpha < 1)
        {
            alpha += fadeSpeed * Time.deltaTime;
            Color currentColour = particlesMat.color;
            currentColour.a = alpha;
            particlesMat.color = currentColour;
            yield return null;
        }
    }*/
}
