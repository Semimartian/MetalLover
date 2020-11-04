using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalScoreText : MonoBehaviour
{

    [SerializeField] private TMPro.TextMeshPro text;
    [SerializeField] private AudioSource audioSource;

    void Start()
    {
      //  PlayAnimation();
    }

    [SerializeField] int finalScore = 1440;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float minimumPopInterval;
    public void PlayAnimation()
    {
        StartCoroutine(textAnimation());
    }

    private IEnumerator textAnimation()
    {

        float currentFloatValue = 0;
        int currentIntValue = (int)currentFloatValue;
        text.text = currentIntValue.ToString();

        //yield return new WaitForSeconds(1);

        float lastPopTime = 0;

        while (currentFloatValue < finalScore)
        {
           // yield return new WaitForSeconds(timePerDigit);
            yield return null;
            /*float increment =
                curve.Evaluate(((currentFloatValue - finalScore) / finalScore) + 1);*/
            float increment =
               curve.Evaluate(currentFloatValue) * Time.deltaTime; 
            currentFloatValue += increment;

            int intValue = (int)currentFloatValue;
            if(intValue != currentIntValue)
            {
                currentIntValue = intValue;
                text.text = currentIntValue.ToString();
                if(Time.time > lastPopTime + minimumPopInterval)
                {
                    audioSource.Play();
                    lastPopTime = Time.time;
                }
            }
        }


    }
}
