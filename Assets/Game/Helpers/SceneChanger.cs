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

    [NonSerialized]
    public Tower targetTower;
            
    public void GoBack()
    {
        ChangeScene((Country)null);
    }

    public void ChangeScene(Country target)
    {
        GameManager.instance.gameStateManager.Save();

        targetCountry = target;
        SceneManager.LoadScene("CountryScene");
    }

    public void ChangeScene(TowerFloor target)
    {
        GameManager.instance.gameStateManager.Save();

        targetFloor = target;
        SceneManager.LoadScene("FloorScene");
    }

    public void ChangeScene(Tower target)
    {
        GameManager.instance.gameStateManager.Save();

        targetTower = target;
        SceneManager.LoadScene("TowerScene");
    }
}