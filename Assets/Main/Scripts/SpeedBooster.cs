using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBooster : MonoBehaviour
{
    [SerializeField] float speedBoost;
    public float SpeedBoost
    {
        get { return speedBoost; }
    }
}
