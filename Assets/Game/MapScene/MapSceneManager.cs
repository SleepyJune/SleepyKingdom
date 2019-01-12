using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

public class MapSceneManager : MonoBehaviour
{
    public static MapSceneManager instance;

    public Tilemap terrainMap;
    public Tilemap overlayMap;

    [NonSerialized]
    public MapSceneCameraController cameraController;

    [NonSerialized]
    public MapUnitManager unitManager;

    [NonSerialized]
    public MapCastleManager castleManager;

    [NonSerialized]
    public MapResourceManager resourceManager;

    void Awake()
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

        Pathfinder.Initialize(terrainMap);

        unitManager = GetComponent<MapUnitManager>();
        resourceManager = GetComponent<MapResourceManager>();
        castleManager = GetComponent<MapCastleManager>();
        cameraController = GetComponent<MapSceneCameraController>();
    }
   

    private void OnDestroy()
    {
        GameManager.instance.gameStateManager.gameState.SaveMapUnits(this);
    }
}