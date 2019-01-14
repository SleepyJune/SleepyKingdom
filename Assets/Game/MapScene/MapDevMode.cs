using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MapDevMode : MonoBehaviour
{

    [NonSerialized]
    public Transform canvas;

    private void Start()
    {
        var canvasGO = GameObject.Find("Canvas");
        if(canvasGO != null)
        {
            canvas = canvasGO.transform;
        }
    }

    private void Update()
    {
        
    }
}