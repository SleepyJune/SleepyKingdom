using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MapUnitManager : MonoBehaviour
{
    public List<MapUnit> allUnits = new List<MapUnit>();
        
    public Transform unitParent;

    public MapCastleUnit castlePrefab;

    private PathfindingManager pathfindingManager;

    public static Dictionary<Vector3Int, GameTile> map;

    //public delegate void OnMouseClickFunction(Unit unit);
    //public event OnMouseClickFunction OnUnitClickEvent;

    public CastleWindowController castleWindow;

    public ActionCircleController actionCircle;

    public GameTileClickHandler gameTileClickHandler;

    private MapCastleUnit myCastle;

    private UnitCommandType currentCommand = UnitCommandType.None;
    private MapCastleUnit currentSelectedUnit = null;

    private void Start()
    {
        GameManager.instance.globalCountryManager.OnAddCountryEvent += OnAddCountryEvent;
        GameManager.instance.globalCountryManager.OnDeleteCountryEvent += OnDeleteCountryEvent;

        gameTileClickHandler.OnGameTileClickedEvent += OnGameTileClickedEvent;

        map = Pathfinder.map;

        foreach(var country in GameManager.instance.gameStateManager.gameState.GetCountries())
        {
            OnAddCountryEvent(country);
        }
    }

    private void OnDestroy()
    {
        GameManager.instance.globalCountryManager.OnAddCountryEvent -= OnAddCountryEvent;
        GameManager.instance.globalCountryManager.OnDeleteCountryEvent -= OnDeleteCountryEvent;

        gameTileClickHandler.OnGameTileClickedEvent -= OnGameTileClickedEvent;
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
        if(currentCommand == UnitCommandType.Move)
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
        if(unit is MapCastleUnit)
        {
            var castle = unit as MapCastleUnit;

            //castleWindow.SetCountry(castle.country);

            if(currentCommand == UnitCommandType.Attack)
            {
                ResetCommand();
                return;
            }

            actionCircle.SetCastle(castle);

        }
    }

    private void OnAddCountryEvent(Country country)
    {
        AddCastleUnit(country);
    }

    public void AddCastleUnit(Country country)
    {
        var newCastle = Instantiate(castlePrefab, unitParent);
        newCastle.country = country;
        newCastle.position = country.position;

        if(country.castleObject != null)
        {
            newCastle.castleObject = country.castleObject;
        }

        if(country.countryID == 1)
        {
            myCastle = newCastle;
        }

        InitializeUnit(newCastle);
    }

    private void OnDeleteCountryEvent(Country country)
    {
        DeleteCastleUnit(country);
    }

    public void DeleteCastleUnit(Country country)
    {        
        foreach(var unit in allUnits)
        {
            var castle = unit as MapCastleUnit;
            if(castle != null)
            {
                if(castle.country.countryID == country.countryID)
                {
                    allUnits.Remove(unit);
                    Destroy(castle.gameObject);
                    break;
                }
            }
        }
    }

    public void InitializeUnit(MapUnit unit)
    {
        unit.unitManager = this;
        
        allUnits.Add(unit);
    }
}