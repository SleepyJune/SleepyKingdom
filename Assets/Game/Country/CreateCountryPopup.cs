using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class CreateCountryPopup : MonoBehaviour
{
    //public Text text;
    public InputField input;

    public Button button;

    public void OnCreateButtonPress()
    {
        var name = input.text;
        GameManager.instance.globalCountryManager.OnCreateCountry(name);

        Destroy(gameObject);
    }
}