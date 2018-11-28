using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "Game/Upgrades/Country Upgrade")]
public class CountryUpgradeObject : UpgradeObject
{
    public string targetString;

    public void ApplyUpgrade(Country country, int level)
    {
        if (level < maxLevel)
        {
            float newValue = 10;

            var targetField = country.GetType().GetField(targetString);

            targetField.SetValue(country, System.Convert.ChangeType(newValue, targetField.FieldType));
        }
    }
}