using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    
    [NonSerialized]
    public GameDatabaseManager gamedatabaseManager;

    [NonSerialized]
    public GameStateManager gameStateManager;

    [NonSerialized]
    public SceneChanger sceneChanger;

    [NonSerialized]
    public EventManager eventManager;

    [NonSerialized]
    public WeatherManager weatherManager;

    [NonSerialized]
    public GlobalCountryManager globalCountryManager;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        gameStateManager = GetComponent<GameStateManager>();
        gamedatabaseManager = GetComponent<GameDatabaseManager>();
        sceneChanger = GetComponent<SceneChanger>();
        eventManager = GetComponent<EventManager>();
        weatherManager = GetComponent<WeatherManager>();
        globalCountryManager = GetComponent<GlobalCountryManager>();
    }

    private void OnApplicationQuit()
    {
        gameStateManager.Save();
    }
}