using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class CastleWindowController : Popup
{
    public Text countryName;

    [NonSerialized]
    public Country country;

    public ResourceButtonUI resourceButtonUIPrefab;

    public Transform resourceParent;
    
    public void SetCountry(Country targetCountry)
    {
        Show();

        country = targetCountry;

        countryName.text = country.countryName;

        //countryPopulation.text = country.Population.ToString();
        //country.OnResourceChange += OnResourceChange;

        //GenerateResourceUI("Population", country.population);
        //GenerateResourceUI("Happiness", country.happiness);

        foreach(Transform resource in resourceParent)
        {
            Destroy(resource.gameObject);
        }

        GenerateResourceUI(CountryResourceType.Wood);
        GenerateResourceUI(CountryResourceType.Stone);
        GenerateResourceUI(CountryResourceType.Wheat);
        GenerateResourceUI(CountryResourceType.Gold);
    }

    public void GenerateResourceUI(CountryResourceType type)
    {
        var newResource = Instantiate(resourceButtonUIPrefab, resourceParent);

        newResource.resourceName.text = type.ToString();
        newResource.SetResource(country, type);
    }

    public void DeleteCastle()
    {
        if(country != null)
        {
            GameManager.instance.globalCountryManager.DeleteCountry(country);
        }
    }
}