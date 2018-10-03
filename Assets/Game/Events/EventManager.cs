using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class EventManager : MonoBehaviour
{
    public EventDatabase eventDatabase;

    public Dictionary<string, EventObject> eventObjects = new Dictionary<string, EventObject>();

    [NonSerialized]
    public List<EventObject> weatherEventObjects = new List<EventObject>();

    public delegate void ProcessGameEvent(Event gameEvent);
    public event ProcessGameEvent OnNewWeatherEvent;

    private void Awake()
    {
        foreach(var gameEvent in eventDatabase.allEvents)
        {
            eventObjects.Add(gameEvent.eventName, gameEvent);

            if(gameEvent.eventType == GameEventType.Weather)
            {
                weatherEventObjects.Add(gameEvent);
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