using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    Map,
    Floor,
    Tower,
    Country,
    Market,
    CashShop,
    Temple,
}

public class SceneChanger : MonoBehaviour
{
    [NonSerialized]
    public Country targetCountry;

    [NonSerialized]
    public TowerFloor targetFloor;

    [NonSerialized]
    public Tower targetTower;

    public static SceneType currentScene;

    public void Start()
    {
        var current = SceneManager.GetActiveScene();
        currentScene = (SceneType)current.buildIndex;

        SceneManager.activeSceneChanged += OnChangeActiveScene;
    }

    private void OnChangeActiveScene(Scene current, Scene next)
    {
        currentScene = (SceneType)next.buildIndex;
    }

    public void GoBack()
    {
        ChangeScene((Country)null);
    }

    public void ChangeScene(SceneType sceneType)
    {
        if(currentScene == sceneType)
        {
            return;
        }

        if(sceneType == SceneType.Map)
        {
            SceneManager.LoadScene("MapScene");
        }
        else if (sceneType == SceneType.Market)
        {
            SceneManager.LoadScene("MarketScene");
        }
        else if (sceneType == SceneType.Country)
        {
            SceneManager.LoadScene("CountryScene");
        }
        else if (sceneType == SceneType.CashShop)
        {
            SceneManager.LoadScene("CashShopScene");
        }
        else if (sceneType == SceneType.Temple)
        {
            SceneManager.LoadScene("TempleScene");
        }
    }

    public void ChangeScene(Country target)
    {
        GameManager.instance.gameStateManager.Save();

        targetCountry = target;
        SceneManager.LoadScene("CountryScene");
    }

    public void ChangeScene(TowerFloor target)
    {
        GameManager.instance.gameStateManager.Save();

        targetFloor = target;
        SceneManager.LoadScene("FloorScene");
    }

    public void ChangeScene(Tower target)
    {
        GameManager.instance.gameStateManager.Save();

        targetTower = target;
        SceneManager.LoadScene("TowerScene");
    }
}