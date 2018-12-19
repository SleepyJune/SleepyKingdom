using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class DeployUnitButton : MonoBehaviour
{
    BattleUnitObject unitObj;

    DeployUnitManager unitManager;

    public Image icon;

    public void Initialize(BattleUnitObject unitObj, DeployUnitManager manager)
    {
        this.unitObj = unitObj;
        unitManager = manager;

        icon.sprite = unitObj.image;
    }

    public void OnButtonPressed()
    {
        unitManager.OnUnitButtonPressed(unitObj);
    }
}