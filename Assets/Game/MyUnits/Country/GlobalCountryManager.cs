using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

public class GlobalCountryManager : MonoBehaviour
{    
    private float lastUpdateTime;
    private float updateFrequency = 1;

    private GameState gameState;
    
    public delegate void CountryEventFunction(Country country);
    public event CountryEventFunction OnAddCountryEvent;
    public event CountryEventFunction OnDeleteCountryEvent;

    [NonSerialized]
    public MyCountry myCountry;

    private void Awake()
    {
        gameState = GameManager.instance.gameStateManager.gameState;

        InitiateCountries();
    }

    private void InitiateCountries()
    {
        if (!gameState.isInitialized())
        {
            return;
        }

        Country.countryCounter = gameState.countryCounter;

        var countries = gameState.GetCountries();

        myCountry = gameState.myCountry;

        if(myCountry == null)
        {
            myCountry = new MyCountry("New Country");

            //should jump to create country scene
        }

        List<Country> countryToDelete = new List<Country>();

        foreach (var country in countries)
        {
            if (!GameManager.instance.gamedatabaseManager.currentMap.countryDataObjectDictionary.ContainsKey(country.countryID))
            {
                //country never existed in the first place, remove it
                countryToDelete.Add(country);                
                continue;
            }

            AddCountry(country, false);
        }

        foreach(var country in countryToDelete)
        {
            DeleteCountry(country);
        }

        AddMapDataCountries();
    }

    void AddMapDataCountries()
    {
        foreach (var tile in GameManager.instance.gamedatabaseManager.currentMap.castleSpawnTileDictionary.Values)
        {            
            var country = tile.countryData.country;
            if (!gameState.CountryExists(country.countryID))
            {
                AddCountry(country);
            }
        }
    }

    public Country GetCountry(int countryID)
    {
        return GameManager.instance.gameStateManager.gameState.GetCountry(countryID);
    }

    public void AddCountry(Country newCountry, bool addToState = true)
    {
        if (addToState)
        {
            GameManager.instance.gameStateManager.gameState.AddCountry(newCountry);
        }

        CountryDataObject countryData;

        if (GameManager.instance.gamedatabaseManager.currentMap.countryDataObjectDictionary.TryGetValue(newCountry.countryID, out countryData))
        {
            newCountry.Initialize(countryData);
        }

        if (OnAddCountryEvent != null)
        {
            OnAddCountryEvent(newCountry);
        }
    }

    public void DeleteCountry(Country country)
    {
        GameManager.instance.gameStateManager.gameState.DeleteCountry(country);

        if(OnDeleteCountryEvent != null)
        {
            OnDeleteCountryEvent(country);
        }
    }

    public void OnCreateCountry(string name)
    {
        var country = Country.Generate(name);
        AddCountry(country);
    }

    private void Update()
    {
        UpdateCountryPopulation();

        /*if (Time.time - lastUpdateTime >= updateFrequency)
        {
            

            lastUpdateTime = Time.time;
        }*/
    }

    private void UpdateCountryPopulation()
    {
        foreach(var country in gameState.GetCountries())
        {
            
        }
    }
}