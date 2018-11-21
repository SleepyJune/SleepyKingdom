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
        
    private void Start()
    {
        unitManager = GetComponent<MapUnitManager>();
    }
}