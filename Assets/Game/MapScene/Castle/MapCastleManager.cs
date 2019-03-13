using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MapCastleManager : MonoBehaviour
{
    public MapCastleUnit defaultCastleUnit;

    [NonSerialized]
    public Dictionary<int, MapCastleUnit> castles = new Dictionary<int, MapCastleUnit>();
    
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
    }

    private void OnDestroy()
    {
        GameManager.instance.globalCountryManager.OnAddCountryEvent -= OnAddCountryEvent;
        GameManager.instance.globalCountryManager.OnDeleteCountryEvent -= OnDeleteCountryEvent;
    }

    private void OnAddCountryEvent(Country country)
    {
        AddCastleUnit(country);
    }

    public void AddCastleUnit(Country country)
    {
        var castlePrefab = defaultCastleUnit;

        if (country.castlePrefabId != 0)
        {
            var prefab = GameManager.instance.gamedatabaseManager.GetPrefab<MapCastleUnit>(country.castlePrefabId);
            if(prefab != null)
            {
                castlePrefab = prefab;
            }
        }

        var newCastle = Instantiate(castlePrefab, unitManager.unitParent);
        newCastle.country = country;
        newCastle.position = country.position;

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
