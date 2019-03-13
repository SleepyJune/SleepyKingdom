using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class TopUIController : MonoBehaviour
{
    public Text populationText;

    MyCountry country;

    private void Start()
    {
        country = GameManager.instance.globalCountryManager.myCountry;
    }

    private void Update()
    {
        populationText.text = NumberTextFormater.FormatNumber(country.population);//Mathf.Round(country.population).ToString();
    }
}