using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class CreateCountryPopup : Popup
{
    //public Text text;
    public InputField input;

    public Button button;

    [NonSerialized]
    public string inputText;

    public void OnCreateButtonPress()
    {
        inputText = input.text;
        //GameManager.instance.globalCountryManager.OnCreateCountry(inputText);

        Close();
    }
}