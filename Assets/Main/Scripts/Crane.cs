using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crane : MonoBehaviour
{
    [SerializeField] AnimationCurve curve;
    //[SerializeField] Transform highPoint;
    //[SerializeField] Transform lowPoint;
    [SerializeField] Magnet magnet;
    private float timer;
    private float yOffset;
    private void Start()
    {
        yOffset = magnet.transform.position.y;
    }


    private void FixedUpdate()
    {
        timer += Time.deltaTime;
        if(timer>= curve.keys[curve.length-1].time)
        {
            timer = 0;
        }
        Vector3 newMagnetPosition = magnet.transform.position;
        newMagnetPosition.y = curve.Evaluate(timer) + yOffset;

        magnet.MovePhysically(newMagnetPosition);
       // magnet.rigidbody.MovePosition(newMagnetPosition);
    }
}
