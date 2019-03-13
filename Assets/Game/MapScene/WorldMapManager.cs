using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldMapManager : MonoBehaviour
{
    [NonSerialized]
    public string currentMapName;

    private Transform mapParent;

    public string defaultMap = "PEI";

    private MapUnitManager unitManager;

    public void Start()
    {
        unitManager = GetComponent<MapUnitManager>();

        mapParent = GameObject.Find("TerrainMap").transform;

        Invoke("Initialize", .05f);
    }

    void Initialize()
    {
        currentMapName = GameManager.instance.gameStateManager.gameState.currentMapName;

        currentMapName = unitManager.myShip.ship.mapName;

        if (!ChangeMap(currentMapName)) //if changing map failed
        {
            ChangeMap(defaultMap);
        }
    }

    public bool ChangeMap(string mapName)
    {
        if (mapName == null || mapName == "")
        {
            return false;
        }

        Debug.Log("Changing map to " + mapName);

        if( InitTilemap(mapName) &&
            InitInteractables(mapName))
        {
            unitManager.myShip.ship.mapName = mapName;
            unitManager.cameraController.CenterMyShip();

            currentMapName = mapName;
            return true;
        }

        return false;
    }

    bool InitTilemap(string mapName)
    {
        var mapTransform = mapParent.Find(mapName);

        if (mapTransform)
        {
            DisableAllChildren();
            mapTransform.gameObject.SetActive(true);

            var tilemap = mapTransform.GetComponent<Tilemap>();

            if (tilemap)
            {
                Pathfinder.Initialize(tilemap);
                return true;
            }
        }

        return false;
    }

    bool InitInteractables(string mapName)
    {
        if (GameManager.instance.gamedatabaseManager.ChangeMap(mapName))
        {
            unitManager.interactableManager.Initialize();

            var portal = unitManager.interactableManager.FindPortal(currentMapName);
            if (portal)
            {
                unitManager.myShip.SetPosition(portal.position);
            }

            return true;
        }
        else
        {
            unitManager.interactableManager.Unload();
            return false;
        }

        return false;
    }

    void DisableAllChildren()
    {
        foreach (Transform child in mapParent)
        {
            child.gameObject.SetActive(false);
        }
    }
}
