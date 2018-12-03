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

    public CreateCountryPopup createCountryPrefab;

    public delegate void CountryEventFunction(Country country);
    public event CountryEventFunction OnAddCountryEvent;
    public event CountryEventFunction OnDeleteCountryEvent;

    [NonSerialized]
    public Country myCountry;

    private void Awake()
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
            if (Random.Range(0, 1) <= country.matingRate)
            {
                country.population += (.2f * country.population * country.birthRate) * Time.deltaTime;

                if(country.population > country.maxCapacity)
                {
                    country.population = country.maxCapacity;
                }

            }

            if (Random.Range(0, 1) <= country.deathRate)
            {
                country.population -= .1f *(country.population) * Time.deltaTime;

                if(country.population < 0)
                {
                    country.population = 0;
                }
            }

            country.wood += country.population / 1000.0f;
            country.stone += country.population / 1000.0f;
            country.wheat += 2 * country.population / 1000.0f;
            country.gold += country.population / 1000.0f;
            country.water += country.population / 1000.0f;

            //food consumption
            if(Random.Range(0, 1) <= country.foodConsumptionRate)
            {
                country.water -= (country.population / 1000.0f) * Time.deltaTime;
            }

            if (Random.Range(0, 1) <= country.waterConsumptionRate)
            {
                country.wheat -= (country.population / 1000.0f) * Time.deltaTime;
            }
        }
    }
}