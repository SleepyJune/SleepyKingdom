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

    private void Start()
    {
        GameManager.instance.globalCountryManager.OnAddCountryEvent += OnAddCountryEvent;

        map = Pathfinder.map;
    }

    private void OnDestroy()
    {
        GameManager.instance.globalCountryManager.OnAddCountryEvent -= OnAddCountryEvent;
    }

    public void OnUnitMouseClickEvent(Unit unit)
    {
        if(unit is CastleUnit)
        {
            var castle = unit as CastleUnit;

            castleWindow.SetCountry(castle.country);
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

        InitializeUnit(newCastle);
    }

    public void InitializeUnit(Unit unit)
    {
        unit.unitManager = this;
        unit.SetPosition(unit.position);

        allUnits.Add(unit);
    }
}