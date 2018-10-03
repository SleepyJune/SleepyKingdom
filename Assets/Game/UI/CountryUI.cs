using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class CountryUI : MonoBehaviour
{
    public Text countryName;

    public Country country;

    public ResourceButtonUI resourceButtonUIPrefab;

    public Transform resourceParent;

    public Text countryPopulation;

    private void Start()
    {
        if (GameManager.instance.sceneChanger)
        {
            var targetCountry = GameManager.instance.sceneChanger.targetCountry;

            if (targetCountry != null)
            {
                SetCountry(targetCountry);
            }
        }
        else
        {
            var targetCountry = Country.Generate();

            SetCountry(targetCountry);
        }
    }

    private void SetCountry(Country targetCountry)
    {
        country = targetCountry;

        countryName.text = country.countryName;

        countryPopulation.text = country.population.ToString();

        //GenerateResourceUI("Population", country.population);
        //GenerateResourceUI("Happiness", country.happiness);

        GenerateResourceUI("Wood", country.wood);
        GenerateResourceUI("Stone", country.stone);
        GenerateResourceUI("Wheat", country.wheat);
        GenerateResourceUI("Gold", country.gold);
    }

    public void GenerateResourceUI(string name, int value)
    {
        var newResource = Instantiate(resourceButtonUIPrefab, resourceParent);

        newResource.resourceName.text = name;
        newResource.UpdateResource(value);
    }
}