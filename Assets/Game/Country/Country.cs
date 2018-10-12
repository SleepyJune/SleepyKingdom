using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Random = UnityEngine.Random;

public enum CountryResourceType
{
    Population,
    Wood,
    Stone,
    Wheat,
    Gold,
    TaxRate,
    Happiness,
}

[Serializable]
public class Country
{
    public int countryID;

    public string countryName;

    public Tower tower;

    //leader

    //private resources
    private float _population;

    private float _wood;
    private float _stone;
    private float _wheat;
    private float _gold;

    private float _taxRate;
    private float _happiness;

    [NonSerialized]
    static int countryCounter;

    private Dictionary<CountryResourceType, float> resourcesDictionary = new Dictionary<CountryResourceType, float>();

    public float Population
    {
        get { return _population; }
        set { ResourceChange(CountryResourceType.Population, _population, value); _population = value; }
    }

    public float Wood
    {
        get { return _wood; }
        set { ResourceChange(CountryResourceType.Wood, _wood, value); _wood = value; }
    }

    public float Stone
    {
        get { return _stone; }
        set { ResourceChange(CountryResourceType.Stone, _stone, value); _stone = value; }
    }

    public float Wheat
    {
        get { return _wheat; }
        set { ResourceChange(CountryResourceType.Wheat, _wheat, value); _wheat = value; }
    }

    public float Gold
    {
        get { return _gold; }
        set { ResourceChange(CountryResourceType.Gold, _gold, value); _gold = value; }
    }

    public float TaxRate
    {
        get { return _taxRate; }
        set { ResourceChange(CountryResourceType.TaxRate, _taxRate, value); _taxRate = value; }
    }

    public float Happiness
    {
        get { return _happiness; }
        set { ResourceChange(CountryResourceType.Happiness, _happiness, value); _happiness = value; }
    }

    public delegate void ProcessResourceEvent(CountryResourceType type, float oldValue, float newValue);
    public event ProcessResourceEvent OnResourceChange;

    private void ResourceChange(CountryResourceType type, float oldValue, float newValue)
    {
        resourcesDictionary[type] = newValue;

        if (OnResourceChange != null)
        {
            OnResourceChange(type, oldValue, newValue);
        }
    }

    public float GetResource(CountryResourceType type)
    {
        return resourcesDictionary[type];
    }

    public static Country Generate()
    {
        var newCountry = new Country();

        newCountry.countryID = GetCountryCounter();

        newCountry.countryName = "New Country " + newCountry.countryID;

        newCountry.tower = Tower.Generate();

        newCountry.Population = Random.Range(100, 1000);

        newCountry.Wood = Random.Range(1, 100);
        newCountry.Stone = Random.Range(1, 100);
        newCountry.Wheat = Random.Range(1, 100);
        newCountry.Gold = Random.Range(1, 100);

        newCountry.TaxRate = Random.Range(0, 100);

        newCountry.Happiness = Random.Range(1, 100);
                
        return newCountry;
    }

    public static int GetCountryCounter()
    {
        return countryCounter++;
    }
}
