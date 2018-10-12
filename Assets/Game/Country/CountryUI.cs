using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class CountryUI : MonoBehaviour
{
    public Text countryName;

    [NonSerialized]
    public Country country;

    public ResourceButtonUI resourceButtonUIPrefab;

    public Transform resourceParent;

    public Text countryPopulation;

    public Image weatherImage;

    private void Start()
    {
        if (GameManager.instance.sceneChanger)
        {
            country = GameManager.instance.sceneChanger.targetCountry;
        }

        var gameState = GameManager.instance.gameStateManager.gameState;

        if (gameState != null)
        {
            var list = gameState.GetCountries();
            if(list.Count > 0)
            {
                country = list[0];
            }
        }


        if(country == null || country.countryID == 0)
        {
            country = Country.Generate();

            GameManager.instance.gameStateManager.gameState.AddCountry(country);            
        }

        SetCountry(country);

        GameManager.instance.eventManager.OnNewWeatherEvent += OnNewWeatherEvent;
    }

    private void OnDestroy()
    {
        GameManager.instance.eventManager.OnNewWeatherEvent -= OnNewWeatherEvent;
        country.OnResourceChange -= OnResourceChange;
    }

    public void OnWeatherIconClick()
    {
        GameManager.instance.weatherManager.ChangeWeather();
    }

    private void OnNewWeatherEvent(Event gameEvent)
    {
        WeatherEvent newWeather = gameEvent as WeatherEvent;

        weatherImage.sprite = newWeather.image;
    }

    private void SetCountry(Country targetCountry)
    {
        country = targetCountry;

        countryName.text = country.countryName;

        countryPopulation.text = country.Population.ToString();
        country.OnResourceChange += OnResourceChange;

        //GenerateResourceUI("Population", country.population);
        //GenerateResourceUI("Happiness", country.happiness);

        GenerateResourceUI(CountryResourceType.Wood);
        GenerateResourceUI(CountryResourceType.Stone);
        GenerateResourceUI(CountryResourceType.Wheat);
        GenerateResourceUI(CountryResourceType.Gold);
    }

    private void OnResourceChange(CountryResourceType type, float oldValue, float newValue)
    {
        if(type == CountryResourceType.Population)
        {
            countryPopulation.text = Mathf.Round(newValue).ToString();
        }
    }

    public void GenerateResourceUI(CountryResourceType type)
    {
        var newResource = Instantiate(resourceButtonUIPrefab, resourceParent);

        newResource.resourceName.text = type.ToString();
        newResource.SetResource(country, type);

        //newResource.amountButton.onClick.AddListener(() => { update});
    }

    public void OnTowerButtonPress()
    {
        GameManager.instance.sceneChanger.ChangeScene(country.tower);
    }
}