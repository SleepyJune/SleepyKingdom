using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
        
    [NonSerialized]
    public SceneChanger sceneChanger;

    [NonSerialized]
    public EventManager eventManager;

    [NonSerialized]
    public WeatherManager weatherManager;

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

        sceneChanger = GetComponent<SceneChanger>();
        eventManager = GetComponent<EventManager>();
        weatherManager = GetComponent<WeatherManager>();
    }
}