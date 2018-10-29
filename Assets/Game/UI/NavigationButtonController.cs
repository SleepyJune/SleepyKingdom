using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class NavigationButtonController : MonoBehaviour
{
    private SceneChanger sceneChanger;

    private void Start()
    {
        sceneChanger = GameManager.instance.sceneChanger;
    }

    public void OnMapSceneButtonPress()
    {
        sceneChanger.ChangeScene(SceneType.Map);
    }

    public void OnMarketSceneButtonPress()
    {
        sceneChanger.ChangeScene(SceneType.Market);
    }

    public void OnCountrySceneButtonPress()
    {
        sceneChanger.ChangeScene(SceneType.Country);
    }

    public void OnCashShopButtonPress()
    {
        sceneChanger.ChangeScene(SceneType.CashShop);
    }

    public void OnTempleButtonPress()
    {
        sceneChanger.ChangeScene(SceneType.Temple);
    }
}