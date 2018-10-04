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

public class Country
{
    public int countryID;

    public string countryName;

    //leader

    //private resources
    private int _population;

    private int _wood;
    private int _stone;
    private int _wheat;
    private int _gold;

    private int _taxRate;
    private int _happiness;

    [NonSerialized]
    static int countryCounter;

    [NonSerialized]
    public static List<Country> countries = new List<Country>();

    private Dictionary<CountryResourceType, int> resourcesDictionary = new Dictionary<CountryResourceType, int>();

    public int Population
    {
        get { return _population; }
        set { ResourceChange(CountryResourceType.Population, _population, value); _population = value; }
    }

    public int Wood
    {
        get { return _wood; }
        set { ResourceChange(CountryResourceType.Wood, _wood, value); _wood = value; }
    }

    public int Stone
    {
        get { return _stone; }
        set { ResourceChange(CountryResourceType.Stone, _stone, value); _stone = value; }
    }

    public int Wheat
    {
        get { return _wheat; }
        set { ResourceChange(CountryResourceType.Wheat, _wheat, value); _wheat = value; }
    }

    public int Gold
    {
        get { return _gold; }
        set { ResourceChange(CountryResourceType.Gold, _gold, value); _gold = value; }
    }

    public int TaxRate
    {
        get { return _taxRate; }
        set { ResourceChange(CountryResourceType.TaxRate, _taxRate, value); _taxRate = value; }
    }

    public int Happiness
    {
        get { return _happiness; }
        set { ResourceChange(CountryResourceType.Happiness, _happiness, value); _happiness = value; }
    }

    public delegate void ProcessResourceEvent(CountryResourceType type, int oldValue, int newValue);
    public event ProcessResourceEvent OnResourceChange;

    private void ResourceChange(CountryResourceType type, int oldValue, int newValue)
    {
        resourcesDictionary[type] = newValue;

        if (OnResourceChange != null)
        {
            OnResourceChange(type, oldValue, newValue);
        }
    }

    public int GetResource(CountryResourceType type)
    {
        return resourcesDictionary[type];
    }

    public static Country Generate()
    {
        var newCountry = new Country();

        newCountry.countryID = GetCountryCounter();

        newCountry.countryName = "New Country " + newCountry.countryID;

        newCountry.Population = Random.Range(100, 1000);

        newCountry.Wood = Random.Range(1, 100);
        newCountry.Stone = Random.Range(1, 100);
        newCountry.Wheat = Random.Range(1, 100);
        newCountry.Gold = Random.Range(1, 100);

        newCountry.TaxRate = Random.Range(0, 100);

        newCountry.Happiness = Random.Range(1, 100);

        countries.Add(newCountry);

        return newCountry;
    }

    public static int GetCountryCounter()
    {
        return countryCounter++;
    }
}
