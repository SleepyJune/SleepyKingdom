using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class GlobalCountryManager : MonoBehaviour
{    
    private float lastUpdateTime;
    private float updateFrequency = 1;

    private GameState gameState;

    public CreateCountryPopup createCountryPrefab;

    public delegate void CountryEventFunction(Country country);
    public event CountryEventFunction OnAddCountryEvent;
    public event CountryEventFunction OnDeleteCountryEvent;

    public Country myCountry;

    private void Start()
    {
        gameState = GameManager.instance.gameStateManager.gameState;

        InitiateCountries();
    }

    private void InitiateCountries()
    {
        Country.countryCounter = gameState.countryCounter;

        var countries = gameState.GetCountries();

        if (countries.Count > 0)
        {
            foreach (var country in countries)
            {
                AddCountry(country, false);

                if(country.countryID == 1)
                {
                    myCountry = country;
                }
            }
        }
        else
        {
            //CreateCountryPopup();
        }
    }

    private void CreateCountryPopup()
    {
        var canvas = GameObject.Find("Canvas");

        if(canvas != null)
        {
            var newPopup = Instantiate(createCountryPrefab, canvas.transform);
        }
    }

    public void AddCountry(Country newCountry, bool addToState = true)
    {
        if (addToState)
        {
            GameManager.instance.gameStateManager.gameState.AddCountry(newCountry);
        }

        if(OnAddCountryEvent != null)
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

    public void OnCreateCountry(string name, CastleObject castle, Vector3Int position)
    {
        var country = Country.Generate(name);
        country.castleObject = castle;
        country.position = position;

        AddCountry(country);
    }

    private void Update()
    {
        if(Time.time - lastUpdateTime >= updateFrequency)
        {
            UpdateCountryPopulation();

            lastUpdateTime = Time.time;
        }
    }

    private void UpdateCountryPopulation()
    {
        foreach(var country in gameState.GetCountries())
        {
            country.Population += 1;

            country.Wood += country.Population / 1000.0f;
            country.Stone += country.Population / 1000.0f;
            country.Wheat += 2 * country.Population / 1000.0f;
            country.Gold += country.Population / 1000.0f;

            country.Wheat -= country.Population / 1000.0f;
        }
    }
}