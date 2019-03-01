using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MapUnitManager : MonoBehaviour
{
    [NonSerialized]
    public MapDatabaseObject mapDatabase;

    [NonSerialized]
    public Dictionary<int, MapUnit> allUnits = new Dictionary<int, MapUnit>();

    public Transform popupParent;

    private int unitCounter = 0;

    public Transform unitParent;

    public static Dictionary<Vector3Int, GameTile> map;
        
    public ActionBarController actionBar;

    [NonSerialized]
    public MapCastleUnit myCastle;

    [NonSerialized]
    public MapSceneInputManager inputManager;

    private void Start()
    {
        //gameTileClickHandler.OnGameTileClickedEvent += OnGameTileClickedEvent;
        inputManager = GetComponent<MapSceneInputManager>();

        mapDatabase = GameManager.instance.gamedatabaseManager.currentMap;
        map = Pathfinder.map;
    }

    private void OnDestroy()
    {

    }

    public void DeleteUnit(MapUnit unit)
    {
        allUnits.Remove(unit.unitId);

        Destroy(unit.gameObject);
    }

    public void InitializeUnit(MapUnit unit)
    {
        unit.unitManager = this;
        unit.unitId = unitCounter;

        allUnits.Add(unitCounter, unit);

        unitCounter += 1;
    }
}