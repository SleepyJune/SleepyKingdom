using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GlobalCountryManager : MonoBehaviour
{    
    private float lastUpdateTime;
    private float updateFrequency = 1;

    private GameState gameState;

    private void Start()
    {
        gameState = GameManager.instance.gameStateManager.gameState;
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