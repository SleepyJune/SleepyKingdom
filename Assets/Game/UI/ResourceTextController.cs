using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ResourceTextController : MonoBehaviour
{
    public CountryResourceType resourceType;
    public Text text;

    Country country;

    private void Start()
    {
        Invoke("Initialize", 0.05f);
    }

    private void Initialize()
    {
        country = GameManager.instance.globalCountryManager.myCountry;
        country.OnResourceChange += OnResourceChange;

        text.text = Mathf.Round(country.GetResource(resourceType)).ToString();
    }

    private void OnDestroy()
    {
        country.OnResourceChange -= OnResourceChange;
    }

    private void OnResourceChange(CountryResourceType type, float oldValue, float newValue)
    {
        if (type == resourceType)
        {
            text.text = Mathf.Round(newValue).ToString();
        }
    }
}