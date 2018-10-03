using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Random = UnityEngine.Random;

public enum WeatherType
{
    Sunny,
    Rainy,
    Flood,
    Cold,
    Snowing,
    Storm,
    Draught,
    Tornado,
    Fog,
}

public class WeatherManager : MonoBehaviour
{
    [NonSerialized]
    public int weatherChangeTime = 10; //every 10 seconds

    [NonSerialized]
    public float lastCheckTime = 0;

    [NonSerialized]
    public WeatherEvent currentWeather;

    [NonSerialized]
    public EventManager eventManager;

    public Dictionary<string, Sprite> weatherPictures = new Dictionary<string, Sprite>();
        
    private void Start()
    {
        eventManager = GameManager.instance.eventManager;
    }

    private void Update()
    {
        if(Time.time - lastCheckTime > weatherChangeTime)
        {
            ChangeWeather();
            lastCheckTime = Time.time;
        }        
    }

    private void ChangeWeather()
    {        
        int randomEventIndex = Random.Range(0, eventManager.weatherEventObjects.Count);

        EventObject eventObject = eventManager.weatherEventObjects[randomEventIndex];

        WeatherEvent newWeather = new WeatherEvent(eventObject);

        currentWeather = newWeather;

        eventManager.NewWeatherEvent(newWeather);
    }
}