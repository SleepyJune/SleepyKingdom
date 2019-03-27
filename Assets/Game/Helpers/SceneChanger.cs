using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    Map,
    Country,
    Market,
    CashShop,
    Temple,
    Upgrade,
    NewGame,
}

public class SceneChanger : MonoBehaviour
{
    [NonSerialized]
    public Country targetCountry;
    
    public static SceneType currentScene;

    public Dictionary<string, SceneType> sceneNameDictionary = new Dictionary<string, SceneType>();
    public Dictionary<SceneType, string> sceneTypeDictionary = new Dictionary<SceneType, string>();

    public void Start()
    {
        InitDictionary();

        var current = SceneManager.GetActiveScene();
        currentScene = SceneType.Map;
    }

    public void GoBack()
    {
        ChangeScene((Country)null);
    }

    void InitDictionary()
    {
        sceneNameDictionary.Add("MapScene", SceneType.Map);
        sceneNameDictionary.Add("MarketScene", SceneType.Market);
        sceneNameDictionary.Add("CountryScene", SceneType.Country);
        sceneNameDictionary.Add("CashShopScene", SceneType.CashShop);
        sceneNameDictionary.Add("TempleScene", SceneType.Temple);
        sceneNameDictionary.Add("UpgradeScene", SceneType.Upgrade);
        sceneNameDictionary.Add("CreationScene", SceneType.NewGame);

        foreach(var pair in sceneNameDictionary)
        {
            sceneTypeDictionary.Add(pair.Value, pair.Key);
        }
    }

    public void ChangeScene(SceneType sceneType)
    {
        if(currentScene == sceneType)
        {
            return;
        }

        if(currentScene != SceneType.Map)
        {
            SceneManager.UnloadSceneAsync(sceneTypeDictionary[currentScene]);
        }

        currentScene = sceneType;

        if (sceneType != SceneType.Map)
        {
            SceneManager.LoadScene(sceneTypeDictionary[sceneType], LoadSceneMode.Additive);
        }
    }

    public void ChangeScene(Country target)
    {
        GameManager.instance.gameStateManager.Save();

        targetCountry = target;
        SceneManager.LoadScene("CountryScene");
    }
}