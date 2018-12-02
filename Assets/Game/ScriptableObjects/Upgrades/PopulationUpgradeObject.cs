using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "Game/Upgrades/Population Upgrade")]
public class PopulationUpgradeObject : CountryUpgradeObject
{
    public enum PopulationUpgradeType
    {
        MatingRate,
        BirthRate,
        DeathRate,
    }

    public PopulationUpgradeType upgradeType;

    public override bool Upgrade(object obj)
    {
        Revert(obj);

        Country country = obj as Country;

        int level = PlayerPrefs.GetInt("Upgrades_" + name, 0);

        PlayerPrefs.SetInt("Upgrades_" + name, level + 1);

        Apply(obj);

        return true;
    }

    public override bool Apply(object obj)
    {
        Country country = obj as Country;

        int level = PlayerPrefs.GetInt("Upgrades_" + name, 0);

        if(level == 0)
        {
            return false;
        }

        if(upgradeType == PopulationUpgradeType.MatingRate)
        {
            country.matingRate += (baseStatValue + valueIncrement * level);
        }
        else if (upgradeType == PopulationUpgradeType.BirthRate)
        {
            country.birthRate += (baseStatValue + valueIncrement * level);
        }
        else if (upgradeType == PopulationUpgradeType.DeathRate)
        {
            country.deathRate += (baseStatValue + valueIncrement * level);
        }

        return true;
    }

    public override bool Revert(object obj)
    {
        Country country = obj as Country;

        int level = PlayerPrefs.GetInt("Upgrades_" + name, 0);

        if (level == 0)
        {
            return false;
        }

        if (upgradeType == PopulationUpgradeType.MatingRate)
        {
            country.matingRate -= (baseStatValue + valueIncrement * level);
        }
        else if (upgradeType == PopulationUpgradeType.BirthRate)
        {
            country.birthRate -= (baseStatValue + valueIncrement * level);
        }
        else if (upgradeType == PopulationUpgradeType.DeathRate)
        {
            country.deathRate -= (baseStatValue + valueIncrement * level);
        }

        return true;
    }

    public override string GetDescription(object obj)
    {
        Country country = obj as Country;

        int level = PlayerPrefs.GetInt("Upgrades_" + name, 0);

        if (upgradeType == PopulationUpgradeType.MatingRate ||
            upgradeType == PopulationUpgradeType.BirthRate ||
            upgradeType == PopulationUpgradeType.DeathRate)
        {
            return "Current increase: +" + (baseStatValue + 100 * valueIncrement * level).ToString() + "%";
        }

        return "";
    }
}