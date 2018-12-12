using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class UpgradeItem : MonoBehaviour
{
    [NonSerialized]
    public CountryUpgradeObject upgradeObject;

    public Image itemIcon;
    public Text itemName;

    public Text itemDescription;

    Country country;

    public void SetItem(CountryUpgradeObject item)
    {
        country = GameManager.instance.globalCountryManager.myCountry;

        upgradeObject = item;

        if (upgradeObject.upgradeType == UpgradeObject.UpgradeType.Increment)
        {
            upgradeObject.Apply(country); //change this later
        }

        RefreshItem();
    }

    private void RefreshItem()
    {
        itemIcon.sprite = upgradeObject.image;
        itemName.text = upgradeObject.name + " Lv." + upgradeObject.GetLevel();
        itemDescription.text = upgradeObject.upgradeDescription + "\n" + upgradeObject.GetDescription(country);
    }

    public void OnUpgradeButtonPressed()
    {
        upgradeObject.Upgrade(country);
        RefreshItem();
    }
}