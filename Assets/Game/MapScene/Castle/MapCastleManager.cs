using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MapCastleManager : MonoBehaviour
{
    [NonSerialized]
    public Dictionary<int, MapCastleUnit> castles = new Dictionary<int, MapCastleUnit>();

    public MapCastleUnit castlePrefab;
    
    [NonSerialized]
    public MapCastleUnit myCastle;

    MapUnitManager unitManager;

    private void Start()
    {
        GameManager.instance.globalCountryManager.OnAddCountryEvent += OnAddCountryEvent;
        GameManager.instance.globalCountryManager.OnDeleteCountryEvent += OnDeleteCountryEvent;

        unitManager = GetComponent<MapUnitManager>();

        Initialize();
    }

    private void Initialize()
    {
        foreach(var country in GameManager.instance.gameStateManager.gameState.GetCountries())
        {
            OnAddCountryEvent(country);
        }

        foreach (var save in GameManager.instance.gameStateManager.gameState.mapCastles)
        {
            //unitManager.InitializeUnit(unit);

            if (!castles.ContainsKey(save.countryID))
            {
                Load(save);
            }
        }
    }

    private void OnDestroy()
    {
        GameManager.instance.globalCountryManager.OnAddCountryEvent -= OnAddCountryEvent;
        GameManager.instance.globalCountryManager.OnDeleteCountryEvent -= OnDeleteCountryEvent;
    }

    void Load(MapCastleSave save)
    {
        var country = GameManager.instance.globalCountryManager.GetCountry(save.countryID);

        if (country != null)
        {
            var newCastle = Instantiate(castlePrefab, unitManager.unitParent);

            unitManager.InitializeUnit(newCastle);

            newCastle.Load(save, country);

            castles.Add(country.countryID, newCastle);
        }
    }

    private void OnAddCountryEvent(Country country)
    {
        AddCastleUnit(country);
    }

    public void AddCastleUnit(Country country)
    {
        var newCastle = Instantiate(castlePrefab, unitManager.unitParent);
        newCastle.country = country;
        newCastle.position = country.position;

        if (country.castleObject != null)
        {
            newCastle.castleObject = country.castleObject;
        }

        if (country.countryID == 1)
        {
            myCastle = newCastle;
            unitManager.myCastle = newCastle;
        }

        unitManager.InitializeUnit(newCastle);

        castles.Add(country.countryID, newCastle);
    }

    private void OnDeleteCountryEvent(Country country)
    {
        DeleteCastleUnit(country);
    }

    public void DeleteCastleUnit(Country country)
    {
        MapCastleUnit unit;

        if(castles.TryGetValue(country.countryID, out unit))
        {
            castles.Remove(country.countryID);
            unitManager.DeleteUnit(unit);
        }
    }

}
