using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public abstract class UpgradeObject : SpriteObject
{
    public string upgradeName;

    public string upgradeDescription;

    public enum UpgradeType
    {
        Single,
        Increment,
    }

    public UpgradeType upgradeType;

    public float baseStatValue = 0;

    public float valueIncrement = .1f;

    public int baseCost = 100;
    public int costIncreament = 50;
    public float costIncreamentExponential = 2f;

    public int maxLevel = 10;

    public int defaultMockLevel = 5;

    public abstract bool Apply(object obj);
    public abstract bool Upgrade(object obj);
    public abstract bool Revert(object obj);
    public abstract string GetDescription(object obj);

    public int GetLevel()
    {
        return PlayerPrefs.GetInt("Upgrades_" + name, 0);
    }
}