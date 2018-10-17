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
    Building
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