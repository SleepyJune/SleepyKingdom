using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class RecruitItem : MonoBehaviour
{
    [NonSerialized]
    public BattleUnitObject unitObj;

    public Image icon;
    public Image weaponImage;
    public Image handImage;
    
    public Text unitName;

    public Text unitDescription;

    Country country;

    public void SetItem(BattleUnitObject unitObj)
    {
        country = GameManager.instance.globalCountryManager.myCountry;

        this.unitObj = unitObj;

        icon.sprite = unitObj.image;
        weaponImage.sprite = unitObj.weaponObject.image;

        unitName.text = unitObj.name;
    }

    public void OnRecruitButtonPressed()
    {

    }
}