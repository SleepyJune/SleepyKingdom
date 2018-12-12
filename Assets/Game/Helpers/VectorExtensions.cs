using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public static class VectorExtensions
{   
    public static Vector3 RadianToVector(float radian)
    {
        return new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0);
    }

    public static Vector3 DegreeToVector(float degree)
    {
        return RadianToVector(degree * Mathf.Deg2Rad);
    }
}