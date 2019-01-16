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

    public float speed;

    //leader

    //private resources
    
    public float population;
    public float maxCapacity;

    public float wood;
    public float stone;
    public float wheat;
    public float gold;
    public float water;

    public float taxRate;
    public float happiness;

    [NonSerialized]
    public float birthRate = .5f;
    [NonSerialized]
    public float matingRate = .2f;
    [NonSerialized]
    public float deathRate = .05f;
    [NonSerialized]
    public float foodConsumptionRate = .2f;
    [NonSerialized]
    public float waterConsumptionRate = .2f;

    public Territory territory;

    public static int countryCounter = 1;

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

        newCountry.wood = Random.Range(1, 100);
        newCountry.stone = Random.Range(1, 100);
        newCountry.wheat = Random.Range(1, 100);
        newCountry.gold = Random.Range(1, 100);

        newCountry.taxRate = Random.Range(0, 100);

        newCountry.happiness = Random.Range(1, 100);

        return newCountry;
    }

    public static int GetCountryCounter()
    {
        return countryCounter++;
    }

    public override bool Equals(object obj)
    {
        if(obj is Country)
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
