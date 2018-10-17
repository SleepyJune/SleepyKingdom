using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

[Serializable]
public class GameState
{
    public Country[] countries = new Country[0];

    public int gold;
    public int gems;

    [NonSerialized]
    private List<Country> countryList = new List<Country>();

    public void AddCountry(Country newCountry)
    {
        countryList.Add(newCountry);

        countries = countryList.ToArray();        
    }

    public List<Country> GetCountries()
    {
        return countryList;
    }

    public void Save()
    {        
        string str = JsonUtility.ToJson(this);

        if (!Directory.Exists(DataPath.savePath))
        {
            Directory.CreateDirectory(DataPath.savePath);
        }

        var filePath = DataPath.savePath + "save.json";

        File.WriteAllText(filePath, str);
    }

    public static GameState LoadSave()
    {
        return new GameState();
    }
}