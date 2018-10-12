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
        bool hasSetCountry = false;

        if (GameManager.instance.sceneChanger)
        {
            var targetCountry = GameManager.instance.sceneChanger.targetCountry;

            if (targetCountry != null)
            {
                SetCountry(targetCountry);
                hasSetCountry = true;
            }
        }
        
        if(!hasSetCountry)
        {
            var targetCountry = Country.Generate();

            GameManager.instance.gameStateManager.gameState.AddCountry(targetCountry);

            SetCountry(targetCountry);
        }

        GameManager.instance.eventManager.OnNewWeatherEvent += OnNewWeatherEvent;
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
}