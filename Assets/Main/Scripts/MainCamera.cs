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

    // Update is called once per frame
    void FixedUpdate()
    {

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

    private IEnumerator ScaleOffsetFromTargetCoroutine(float by,float seconds)
    {
        float timePassed = 0;

        float fixedYOrigin = movementProperties.fixedY;
        float fixedYTarget = movementProperties.fixedY * by;
        Vector3 offsetOrigin = movementProperties.positionOffsetFromTarget;
        Vector3 offsetTarget = movementProperties.positionOffsetFromTarget * by;
       // bool shoudContinue = true;
        while (timePassed<seconds)
        {
            float t = timePassed / seconds;

            float byDelta = scaleSpeed * Time.fixedDeltaTime;
            movementProperties.fixedY = Mathf.Lerp(fixedYOrigin,fixedYTarget,t) ;
            movementProperties.positionOffsetFromTarget = Vector3.Lerp(offsetOrigin, offsetTarget, t);

            timePassed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        Debug.Log("shoudContinue = false");
       // movementProperties.positionOffsetFromTarget = target;
        movementProperties.fixedY = fixedYTarget;
        movementProperties.positionOffsetFromTarget = offsetTarget;

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