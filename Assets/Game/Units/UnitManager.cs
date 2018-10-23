using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public List<Unit> allUnits = new List<Unit>();
        
    public Transform unitParent;

    public CastleUnit castlePrefab;

    private PathfindingManager pathfindingManager;

    public static Dictionary<Vector3Int, GameTile> map;

    //public delegate void OnMouseClickFunction(Unit unit);
    //public event OnMouseClickFunction OnUnitClickEvent;

    public CastleWindowController castleWindow;

    public GameTileClickHandler gameTileClickHandler;

    private bool myCountrySelected = false;

    private CastleUnit myCastle;

    private void Start()
    {
        GameManager.instance.globalCountryManager.OnAddCountryEvent += OnAddCountryEvent;
        GameManager.instance.globalCountryManager.OnDeleteCountryEvent += OnDeleteCountryEvent;

        gameTileClickHandler.OnGameTileClickedEvent += OnGameTileClickedEvent;

        map = Pathfinder.map;
    }

    private void OnDestroy()
    {
        GameManager.instance.globalCountryManager.OnAddCountryEvent -= OnAddCountryEvent;
        GameManager.instance.globalCountryManager.OnDeleteCountryEvent -= OnDeleteCountryEvent;

        gameTileClickHandler.OnGameTileClickedEvent -= OnGameTileClickedEvent;
    }

    private void OnGameTileClickedEvent(GameTile tile)
    {
        if (myCountrySelected)
        {
            myCastle.SetMovePosition(tile.position);
        }
    }

    public void DeselectMyCountry()
    {
        myCountrySelected = false;
    }

    public void OnUnitMouseClickEvent(Unit unit)
    {
        if(unit is CastleUnit)
        {
            var castle = unit as CastleUnit;

            castleWindow.SetCountry(castle.country);

            if(castle.country.countryID == 1)
            {
                myCountrySelected = true;
            }
            else
            {
                myCountrySelected = false;
            }
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
            var castle = unit as CastleUnit;
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

    public void InitializeUnit(Unit unit)
    {
        unit.unitManager = this;
        
        allUnits.Add(unit);
    }
}