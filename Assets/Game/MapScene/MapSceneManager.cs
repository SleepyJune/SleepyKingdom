using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

public class MapSceneManager : MonoBehaviour
{
    public Tilemap terrainMap;
    public Tilemap overlayMap;

    [NonSerialized]
    public MapUnitManager unitManager;

    /*void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }*/

    private void Start()
    {
        unitManager = GetComponent<MapUnitManager>();

        Pathfinder.Initialize(terrainMap);
    }
}