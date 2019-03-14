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

    public void Start()
    {
        InitDictionary();

        var current = SceneManager.GetActiveScene();
        ChangeCurrentScene(current.name);

        SceneManager.activeSceneChanged += OnChangeActiveScene;        
    }

    private void OnChangeActiveScene(Scene current, Scene next)
    {
        ChangeCurrentScene(next.name);
    }

    void ChangeCurrentScene(string sceneName)
    {
        SceneType sceneType;
        if (sceneNameDictionary.TryGetValue(sceneName, out sceneType))
        {
            currentScene = sceneType;
        }
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
    }

    public void ChangeScene(SceneType sceneType)
    {
        if(currentScene == sceneType)
        {
            return;
        }

        foreach(var pair in sceneNameDictionary)
        {
            if(pair.Value == sceneType)
            {
                SceneManager.LoadScene(pair.Key);
                break;
            }
        }
    }

    public void ChangeScene(Country target)
    {
        GameManager.instance.gameStateManager.Save();

        targetCountry = target;
        SceneManager.LoadScene("CountryScene");
    }
}