using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainMapController : MonoBehaviour
{
    public static TerrainMapController instance;
        
    public Tilemap tilemap;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        //tilemap = transform.Find("Terrain").GetComponent<Tilemap>();
    }
}
