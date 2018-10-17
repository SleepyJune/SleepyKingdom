using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class WeatherImageController : MonoBehaviour
{
    public Image weatherImage;

    private void Start()
    {
        GameManager.instance.eventManager.OnNewWeatherEvent += OnNewWeatherEvent;
    }

    private void OnDestroy()
    {
        GameManager.instance.eventManager.OnNewWeatherEvent -= OnNewWeatherEvent;
    }

    private void OnNewWeatherEvent(Event gameEvent)
    {
        WeatherEvent newWeather = gameEvent as WeatherEvent;

        weatherImage.sprite = newWeather.image;
    }
}