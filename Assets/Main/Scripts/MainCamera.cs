using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Offset
{
    public Offset(Vector3 position , Vector3 angle)
    {
        this.position = position;
        this.angle = angle;
    }

    public Vector3 position;
    public Vector3 angle;

}

[Serializable]
public struct CameraMovementProperties
{
    /*public CameraMovementProperties(Vector3 position, Vector3 angle)
    {
        this.position = position;
        this.angle = angle;
    }*/

    public Vector3 positionOffsetFromTarget;
    public float fixedY;
    public Vector3 angle;

}


public class MainCamera : MonoBehaviour
{
    [SerializeField] private CameraMovementProperties movementProperties;

    [SerializeField] private Transform lookAtTarget;

    [SerializeField] private float xSpeed = 4;

    private Transform myTransform;


    // Start is called before the first frame update
    void Start()
    {
        myTransform = transform;
    }
    [SerializeField] private cakeslice.OutlineEffect outlineEffect;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            outlineEffect.enabled = !outlineEffect.enabled;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(shouldFollow)
        {
            Vector3 targetPosition = lookAtTarget.position;

            Vector3 moveToPosition = new Vector3();

            moveToPosition.y = movementProperties.fixedY;
            moveToPosition.z = targetPosition.z;
            moveToPosition += movementProperties.positionOffsetFromTarget;
            moveToPosition.x = Mathf.Lerp
                (myTransform.position.x, targetPosition.x, Time.deltaTime * xSpeed);

            myTransform.position = moveToPosition;// new Vector3(newX, myTransform.position.y, newZ);
            myTransform.rotation = Quaternion.Euler(movementProperties.angle);
        }
        //myTransform.rotation = offset.angle.eua
    }

    [SerializeField] private float scaleSpeed = 3;

    public void ScaleOffsetFromTarget(float by)
    {
        StartCoroutine(ScaleOffsetFromTargetCoroutine(by,2));
    }

    public void GoBack()
    {
        StartCoroutine(GoBackCoroutine());
    }

    private IEnumerator ScaleOffsetFromTargetCoroutine(float by, float seconds)
    {
        float timePassed = 0;

        float fixedYOrigin = movementProperties.fixedY;
        float fixedYTarget = movementProperties.fixedY * by;
        Vector3 offsetOrigin = movementProperties.positionOffsetFromTarget;
        Vector3 offsetTarget = movementProperties.positionOffsetFromTarget * by;
        // bool shoudContinue = true;
        while (timePassed < seconds)
        {
            float t = timePassed / seconds;

            float byDelta = scaleSpeed * Time.fixedDeltaTime;
            movementProperties.fixedY = Mathf.Lerp(fixedYOrigin, fixedYTarget, t);
            movementProperties.positionOffsetFromTarget = Vector3.Lerp(offsetOrigin, offsetTarget, t);

            timePassed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        Debug.Log("shoudContinue = false");
        // movementProperties.positionOffsetFromTarget = target;
        movementProperties.fixedY = fixedYTarget;
        movementProperties.positionOffsetFromTarget = offsetTarget;

    }

    private IEnumerator GoBackCoroutine()
    {
        yield return new WaitForSeconds(0.8f);

        shouldFollow = false;
        Vector3 direction = -transform.forward;
        float speed = 0.6f;

        while (true)
        {
            transform.position += direction * speed * Time.deltaTime;
            yield return null;
        }

    }

    private bool shouldFollow = true;
    [SerializeField] Transform finalDestination;
    [SerializeField] private AnimationCurve finalDestinationCurve;

    public IEnumerator GoToFinalDestination()
    {
        shouldFollow = false;

        float time = 0;
        float endTime = finalDestinationCurve.keys[finalDestinationCurve.length - 1].time;
        Transform myTransform = transform;

        Vector3 targetPosition = finalDestination.position;
        Quaternion targetRotation = finalDestination.rotation;
        Vector3 originalPosition = myTransform.position;
        Quaternion originalRotation = myTransform.rotation;

        while (time < endTime)
        {
            //float deltaTime = Time.deltaTime;
            time += Time.deltaTime;

            float t = finalDestinationCurve.Evaluate(time);

            myTransform.position = Vector3.Lerp(originalPosition, targetPosition, t);
            myTransform.rotation = Quaternion.Lerp(originalRotation, targetRotation, t);

            yield return null;

        }


        myTransform.position = targetPosition;
        myTransform.rotation = targetRotation;
    }
}
/*Vector3 myEuler = myTransform.rotation.eulerAngles;
float yAngle = Mathf.Lerp(lookAttarget.rotation.eulerAngles.y, myEuler.y,0.2f*Time.deltaTime);*/
/*myTransform.rotation = 
    Quaternion.Lerp(lookAttarget.rotation, myTransform.rotation, 0.8f );*/

//myEuler.y = yAngle;
// myTransform.rotation = Quaternion.Euler(myEuler);
//myTransform.LookAt(lookAttarget);

//myTransform.rotation = Quaternion.Euler(myEuler);