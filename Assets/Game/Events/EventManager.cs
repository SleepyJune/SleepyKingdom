using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class EventManager : MonoBehaviour
{    
    public Dictionary<string, SpriteObject> eventObjects = new Dictionary<string, SpriteObject>();

    [NonSerialized]
    public List<WeatherObject> weatherSpriteObjects = new List<WeatherObject>();

    public delegate void ProcessGameEvent(GameEvent gameEvent);
    public event ProcessGameEvent OnNewWeatherEvent;

    private void Start()
    {
        foreach(var item in GameManager.instance.gamedatabaseManager.spriteObjects.Values)
        {            
            if(item is WeatherObject)
            {
                var weatherObject = item as WeatherObject;
                weatherSpriteObjects.Add(weatherObject);
            }
        }
    }

    public void NewWeatherEvent(WeatherEvent gameEvent)
    {
        if (OnNewWeatherEvent != null)
        {
            OnNewWeatherEvent(gameEvent);
        }
    }
}