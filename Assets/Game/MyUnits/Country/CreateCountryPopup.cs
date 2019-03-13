using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CreateCountryPopup : MonoBehaviour
{
    public Text countryName;

    public GameStateManager gameStateManager;

    public void OnCreatePressed()
    {
        if(countryName.text != "")
        {
            gameStateManager.NewState(countryName.text);
            SceneManager.LoadScene("MapScene");
        }
    }
}
