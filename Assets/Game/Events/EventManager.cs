using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class EventManager : MonoBehaviour
{    
    public Dictionary<string, SpriteObject> eventObjects = new Dictionary<string, SpriteObject>();

    [NonSerialized]
    public List<SpriteObject> weatherSpriteObjects = new List<SpriteObject>();

    [NonSerialized]
    public List<SpriteObject> defaultResourceObjects = new List<SpriteObject>();

    public delegate void ProcessGameEvent(GameEvent gameEvent);
    public event ProcessGameEvent OnNewWeatherEvent;

    private void Start()
    {
        foreach(var gameEvent in GameManager.instance.gamedatabaseManager.spriteObjects.Values)
        {
            eventObjects.Add(gameEvent.name, gameEvent);

            if(gameEvent.spriteType == SpriteObjectType.Weather)
            {
                weatherSpriteObjects.Add(gameEvent);
            }

            if(gameEvent.spriteType == SpriteObjectType.Resource)
            {
                defaultResourceObjects.Add(gameEvent);
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