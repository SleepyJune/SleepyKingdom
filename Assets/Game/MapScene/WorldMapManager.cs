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

        if (!ChangeMap(currentMapName, unitManager.myShip.ship.position)) //if changing map failed
        {
            ChangeMap(unitManager.myShip.ship.lastMapName, unitManager.myShip.ship.lastMapPosition);
        }
    }

    public bool ChangeMap(string mapName, Vector3Int pos = new Vector3Int())
    {
        if (mapName == null || mapName == "")
        {
            return false;
        }

        Debug.Log("Changing map to " + mapName);
                
        if ( InitTilemap(mapName) &&
            InitInteractables(mapName, pos) &&
            InitMapResources() &&
            CheckShipPosition())
        {
            unitManager.myShip.ship.lastMapPosition = unitManager.myShip.ship.position;
            unitManager.myShip.ship.lastMapName = unitManager.myShip.ship.mapName;

            unitManager.myShip.ship.mapName = mapName;
            unitManager.cameraController.CenterMyShip();

            currentMapName = mapName;
            return true;
        }

        return false;
    }

    bool CheckShipPosition()
    {
        if (unitManager.myShip.gameTile == null || unitManager.myShip.gameTile.isBlocked)
        {
            //something went wrong
            //unitManager.worldMapManager.ChangeMap(unitManager.myShip.ship.lastMapName, unitManager.myShip.ship.lastMapPosition);

            return false;
        }

        return true;
    }

    bool InitMapResources()
    {
        unitManager.mapResourcesManager.Unload();
        return true;
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

    bool InitInteractables(string mapName, Vector3Int pos)
    {
        if (GameManager.instance.gamedatabaseManager.ChangeMap(mapName))
        {
            unitManager.interactableManager.Initialize();

            if (pos != new Vector3Int())
            {
                unitManager.myShip.SetPosition(pos);
            }
            else
            {
                var portal = unitManager.interactableManager.FindPortal(currentMapName);
                if (portal)
                {
                    unitManager.myShip.SetPosition(portal.position);
                }
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
