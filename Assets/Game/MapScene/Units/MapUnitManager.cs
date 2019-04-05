using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MapUnitManager : MonoBehaviour
{
    [NonSerialized]
    public WorldMapManager worldMapManager;

    [NonSerialized]
    public MapInteractableManager interactableManager;

    [NonSerialized]
    public MapSceneCameraController cameraController;

    [NonSerialized]
    public MapResourceManager mapResourcesManager;

    [NonSerialized]
    public MapAnimalManager animalManager;

    [NonSerialized]
    public Dictionary<int, MapUnit> allUnits = new Dictionary<int, MapUnit>();

    public Transform popupParent;

    private int unitCounter = 0;

    public Transform unitParent;
            
    public ActionBarController actionBar;

    [NonSerialized]
    public MapShip myShip;

    [NonSerialized]
    public MapSceneInputManager inputManager;

    private void Awake()
    {
        //gameTileClickHandler.OnGameTileClickedEvent += OnGameTileClickedEvent;
        inputManager = GetComponent<MapSceneInputManager>();

        worldMapManager = GetComponent<WorldMapManager>();

        interactableManager = GetComponent<MapInteractableManager>();

        cameraController = GetComponent<MapSceneCameraController>();

        mapResourcesManager = GetComponent<MapResourceManager>();

        animalManager = GetComponent<MapAnimalManager>();
    }

    private void OnDestroy()
    {

    }

    public void DeleteUnit(MapUnit unit)
    {
        allUnits.Remove(unit.unitId);

        Destroy(unit.gameObject);
    }

    public void AddUnit(MapUnit unit)
    {
        unit.unitManager = this;
        unit.unitId = unitCounter;

        allUnits.Add(unitCounter, unit);

        unitCounter += 1;
    }
}