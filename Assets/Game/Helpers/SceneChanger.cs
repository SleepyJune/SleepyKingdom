using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [NonSerialized]
    public Country targetCountry;

    public void ChangeScene(Country country)
    {
        targetCountry = country;
        SceneManager.LoadScene("CountryScene");
    }
}