using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class CastleWindowController : Popup
{
    public Text countryName;

    [NonSerialized]
    public Country country;

    public Text woodText;
    public Text stoneText;
    public Text wheatText;
    public Text waterText;
    
    public void SetCountry(Country targetCountry)
    {
        Show();

        country = targetCountry;

        countryName.text = country.countryName;
    }

    public void DeleteCastle()
    {
        if(country != null)
        {
            GameManager.instance.globalCountryManager.DeleteCountry(country);
        }
    }

    private void Update()
    {
        woodText.text = NumberTextFormater.FormatNumber(country.wood);
        stoneText.text = NumberTextFormater.FormatNumber(country.stone);
        wheatText.text = NumberTextFormater.FormatNumber(country.wheat);
        waterText.text = NumberTextFormater.FormatNumber(country.water);
    }
}