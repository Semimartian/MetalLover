using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private bool isActive = true;
    public bool IsActive
    {
        get { return isActive; }
    }

    public void Expire()
    {
        isActive = false;
    }
}
