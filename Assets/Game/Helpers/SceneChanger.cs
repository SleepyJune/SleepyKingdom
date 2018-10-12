using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [NonSerialized]
    public Country targetCountry;

    [NonSerialized]
    public TowerFloor targetFloor;

    public void ChangeScene(Country target)
    {
        targetCountry = target;
        SceneManager.LoadScene("CountryScene");
    }

    public void ChangeScene(TowerFloor target)
    {
        targetFloor = target;
        SceneManager.LoadScene("CountryScene");
    }
}