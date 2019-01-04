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
    public Image weaponImage;
    public Image helmetImage;
    public Image handImage;

    public void Initialize(BattleUnitObject unitObj, DeployUnitManager manager)
    {
        this.unitObj = unitObj;
        unitManager = manager;

        icon.sprite = unitObj.image;

        weaponImage.sprite = unitObj.weaponObject.image;

        if(unitObj.helmetObject != null)
        {
            helmetImage.gameObject.SetActive(true);
            helmetImage.sprite = unitObj.helmetObject.image;
        }
    }

    public void OnButtonPressed()
    {
        unitManager.OnUnitButtonPressed(unitObj);
    }
}