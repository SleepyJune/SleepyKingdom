using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Random = UnityEngine.Random;

[Serializable]
public class Country
{
    public int countryID;

    public string countryName;

    //public Tower tower;

    public int castlePrefabId;
    public Vector3Int position;

    //private resources

    public float population;
    public float maxCapacity;

    public static int countryCounter = 1;

    public void Initialize(CountryDataObject countryData)
    {

    }

    public static Country Generate(string countryName = "")
    {
        var newCountry = new Country();

        newCountry.countryID = GetCountryCounter();

        newCountry.countryName = "New Country " + newCountry.countryID;

        if (countryName != "")
        {
            newCountry.countryName = countryName;
        }

        //newCountry.tower = Tower.Generate();

        newCountry.population = Random.Range(100, 1000);
        newCountry.maxCapacity = Random.Range(2000, 10000);

        return newCountry;
    }

    public static int GetCountryCounter()
    {
        return countryCounter++;
    }

    public override bool Equals(object obj)
    {
        if (obj is Country)
        {
            return ((Country)obj).countryID == countryID;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return countryID;
    }
}
