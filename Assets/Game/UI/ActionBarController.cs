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
    public MapCastleUnit selectedCastle;

    public CastleWindowController castleWindow;

    public MapCastleOverlay castleOverlay;

    MapUnitManager unitManager;

    private UnitCommandType currentCommand = UnitCommandType.None;

    private void Start()
    {
        unitManager = MapSceneManager.instance.unitManager;

        unitManager.inputManager.OnGameTileClickEvent += OnGameTileClickedEvent;
    }
    
    private void OnDestroy()
    {
        unitManager.inputManager.OnGameTileClickEvent -= OnGameTileClickedEvent;
    }

    public void OnCastleClick(MapCastleUnit castle)
    {
        if(currentCommand == UnitCommandType.None)
        {
            selectedCastle = castle;
            castleOverlay.SetCastle(castle);
            Show();
        }
    }

    public void OnButtonClicked(int actionIndex)
    {
        var actionType = (UnitCommandType)actionIndex;

        currentCommand = actionType;

        if (actionType == UnitCommandType.Attack)
        {
            Debug.Log("Attack");
        }
        else if (actionType == UnitCommandType.Move)
        {

        }
        else if (actionType == UnitCommandType.Inspect)
        {
            castleWindow.SetCountry(selectedCastle.country);
        }

        Hide();
    }

    private void ResetCommand()
    {
        currentCommand = UnitCommandType.None;
        selectedCastle = null;
    }

    private void OnGameTileClickedEvent(GameTile tile)
    {
        if (currentCommand == UnitCommandType.Move && selectedCastle != null)
        {
            selectedCastle.SetMovePosition(tile.position);            
        }

        currentCommand = UnitCommandType.None;
    }
}