using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MapUnitManager : MonoBehaviour
{
    [NonSerialized]
    public MapDatabase mapDatabase;

    [NonSerialized]
    public Dictionary<int, MapUnit> allUnits = new Dictionary<int, MapUnit>();

    public Transform popupParent;

    private int unitCounter = 0;

    public Transform unitParent;

    public static Dictionary<Vector3Int, GameTile> map;
        
    public ActionCircleController actionCircle;

    public CastleWindowController castleWindow;
    
    private UnitCommandType currentCommand = UnitCommandType.None;
    private MapCastleUnit currentSelectedUnit = null;

    [NonSerialized]
    public MapCastleUnit myCastle;

    [NonSerialized]
    public MapSceneInputManager inputManager;

    private void Start()
    {
        //gameTileClickHandler.OnGameTileClickedEvent += OnGameTileClickedEvent;
        inputManager = GetComponent<MapSceneInputManager>();

        mapDatabase = GameManager.instance.gamedatabaseManager.mapDatabase;
        map = Pathfinder.map;
    }

    private void OnDestroy()
    {

    }

    public void OnActionCircleButtonClick(MapCastleUnit castle, UnitCommandType actionType)
    {
        currentSelectedUnit = castle;
        currentCommand = actionType;

        if (actionType == UnitCommandType.Attack)
        {
            Debug.Log("Attack");
        }
        else if (actionType == UnitCommandType.Move)
        {
            
        }
        else if(actionType == UnitCommandType.Inspect)
        {
            castleWindow.SetCountry(castle.country);
        }
    }

    private void ResetCommand()
    {
        currentCommand = UnitCommandType.None;
        currentSelectedUnit = null;
    }

    private void OnGameTileClickedEvent(GameTile tile)
    {
        if (currentCommand == UnitCommandType.Move)
        {
            currentSelectedUnit.SetMovePosition(tile.position);
        }

        ResetCommand();

        /*if (myCountrySelected)
        {
            myCastle.SetMovePosition(tile.position);
        }*/
    }

    public void OnUnitMouseClickEvent(MapUnit unit)
    {
        if (unit is MapCastleUnit)
        {
            var castle = unit as MapCastleUnit;

            //castleWindow.SetCountry(castle.country);

            if (currentCommand == UnitCommandType.Attack)
            {
                ResetCommand();
                return;
            }

            actionCircle.SetCastle(castle);

        }
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