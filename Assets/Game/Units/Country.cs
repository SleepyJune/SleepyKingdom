using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Random = UnityEngine.Random;

public class Country
{
    public int countryID;

    public string countryName;

    //leader

    public int population;

    public int wood;
    public int stone;
    public int wheat;
    public int gold;

    public float taxRate;

    public int happiness;

    [NonSerialized]
    static int countryCounter;

    [NonSerialized]
    public static List<Country> countries = new List<Country>();

    public static Country Generate()
    {
        var newCountry = new Country();

        newCountry.countryID = GetCountryCounter();

        newCountry.countryName = "New Country " + newCountry.countryID;

        newCountry.population = Random.Range(100, 1000);

        newCountry.wood = Random.Range(1, 100);
        newCountry.stone = Random.Range(1, 100);
        newCountry.wheat = Random.Range(1, 100);
        newCountry.gold = Random.Range(1, 100);

        newCountry.taxRate = Random.Range(.0f, .9f);

        newCountry.happiness = Random.Range(1, 100);

        countries.Add(newCountry);

        return newCountry;
    }

    public static int GetCountryCounter()
    {
        return countryCounter++;
    }
}
