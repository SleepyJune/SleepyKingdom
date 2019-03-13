using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldMapManager : MonoBehaviour
{
    [NonSerialized]
    public string currentMapName;

    public Transform mapParent;

    public string defaultMap = "PEI";

    private MapUnitManager unitManager;

    public void Start()
    {
        unitManager = GetComponent<MapUnitManager>();

        Invoke("Initialize", 1);
    }

    void Initialize()
    {
        currentMapName = GameManager.instance.gameStateManager.gameState.currentMapName;

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
