using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

[Serializable]
public class GameState
{
    [SerializeField]
    private Country[] countries = new Country[0];

    public int gold;
    public int gems;

    [NonSerialized]
    private List<Country> countryList = new List<Country>();

    public void AddCountry(Country newCountry)
    {
        countryList.Add(newCountry);

        countries = countryList.ToArray();

        Save();
    }

    public List<Country> GetCountries()
    {
        return countryList;
    }

    private void Initialize()
    {
        foreach(var country in countries)
        {
            country.InitializeResources();

            countryList.Add(country);
        }
    }

    public void Save()
    {        
        string str = JsonUtility.ToJson(this, true);

        if (!Directory.Exists(DataPath.savePath))
        {
            Directory.CreateDirectory(DataPath.savePath);
        }

        var filePath = DataPath.savePath + "save.json";

        File.WriteAllText(filePath, str);        
    }

    public static GameState Load()
    {
        var filePath = DataPath.savePath + "save.json";
        //var textAsset = Resources.Load<TextAsset>(filePath);

        if (File.Exists(filePath))
        {
            var file = File.ReadAllText(filePath);

            var savedState = JsonUtility.FromJson<GameState>(file);

            if (savedState != null)
            {
                savedState.Initialize();

                return savedState;
            }
            else
            {
                Debug.Log("Not a valid save file.");
            }
        }
        else
        {
            Debug.Log("Save file not found at " + filePath);
        }

        return new GameState();
    }
}