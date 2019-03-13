using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum UnitCommandType
{
    Attack = 0,
    Inspect = 1,
    Move = 2,
    Trade = 3,

    None = -1,
}

public class ActionBarController : Popup
{
    [NonSerialized]
    public MapUnit selectedUnit;

    public CastleWindowController castleWindow;

    public MapCastleOverlay castleOverlay;

    public GameObject flagPrefab;
    public GameObject xMarkPrefab;

    [NonSerialized]
    public GameObject myDestinationFlag;

    MapUnitManager unitManager;

    private UnitCommandType currentCommand = UnitCommandType.None;

    private void Start()
    {
        unitManager = MapSceneManager.instance.unitManager;

        unitManager.inputManager.OnGameTileClickEvent += OnGameTileClickedEvent;
    }
    
    private void OnDestroy()
    {
        if(unitManager && unitManager.inputManager)
        {
            unitManager.inputManager.OnGameTileClickEvent -= OnGameTileClickedEvent;
        }
    }

    public void OnUnitClick(MapUnit unit)
    {
        if(currentCommand == UnitCommandType.None)
        {
            if(unit is MapCastleUnit)
            {
                var castle = unit as MapCastleUnit;

                selectedUnit = castle;
                castleOverlay.SetCastle(castle);
                Show();
            }
            else if (unit is MapShip)
            {
                var ship = unit as MapShip;

                selectedUnit = ship;
                Show();
            }
        }
    }

    public void OnButtonClicked(int actionIndex)
    {
        var actionType = (UnitCommandType)actionIndex;

        currentCommand = actionType;

        if (actionType == UnitCommandType.Attack)
        {
            
        }
        else if (actionType == UnitCommandType.Move)
        {
            
        }
        else if (actionType == UnitCommandType.Inspect)
        {
            //castleWindow.SetCountry(selectedUnit.country);
        }

        Hide();
    }

    private void ResetCommand()
    {
        currentCommand = UnitCommandType.None;
        selectedUnit = null;
    }

    private void OnGameTileClickedEvent(GameTile tile, GameClickEventData clickEventData)
    {
        if (currentCommand == UnitCommandType.Move && selectedUnit != null && selectedUnit is MapMobileUnit)
        {
            var mobileUnit = selectedUnit as MapMobileUnit;

            mobileUnit.SetMovePosition(tile.position);
            clickEventData.isUsed = true;
        }

        currentCommand = UnitCommandType.None;
    }
}