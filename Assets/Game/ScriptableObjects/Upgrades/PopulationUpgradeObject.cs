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
        MaxCapacity,
    }

    public PopulationUpgradeType upgradeTarget;

    public override bool Upgrade(object obj)
    {
        Revert(obj);

        int level = PlayerPrefs.GetInt("Upgrades_" + name, 0);
        PlayerPrefs.SetInt("Upgrades_" + name, level + 1);

        Apply(obj);

        return true;
    }

    public override bool Apply(object obj)
    {
        Country country = obj as Country;

        int level = 0;

        if (upgradeType == UpgradeType.Increment)
        {
            level = PlayerPrefs.GetInt("Upgrades_" + name, 0);

            if (level == 0)
            {
                return false;
            }
        }

        if(upgradeTarget == PopulationUpgradeType.MatingRate)
        {
            country.matingRate += (baseStatValue + valueIncrement * level);
        }
        else if (upgradeTarget == PopulationUpgradeType.BirthRate)
        {
            country.birthRate += (baseStatValue + valueIncrement * level);
        }
        else if (upgradeTarget == PopulationUpgradeType.DeathRate)
        {
            country.deathRate += (baseStatValue + valueIncrement * level);
        }

        if(upgradeType == UpgradeType.Single)
        {
            if(upgradeTarget == PopulationUpgradeType.MaxCapacity)
            {
                country.maxCapacity += baseStatValue;
            }
        }

        return true;
    }

    public override bool Revert(object obj)
    {
        if(upgradeType == UpgradeType.Single)
        {
            return false;
        }

        Country country = obj as Country;

        int level = PlayerPrefs.GetInt("Upgrades_" + name, 0);

        if (level == 0)
        {
            return false;
        }

        if (upgradeTarget == PopulationUpgradeType.MatingRate)
        {
            country.matingRate -= (baseStatValue + valueIncrement * level);
        }
        else if (upgradeTarget == PopulationUpgradeType.BirthRate)
        {
            country.birthRate -= (baseStatValue + valueIncrement * level);
        }
        else if (upgradeTarget == PopulationUpgradeType.DeathRate)
        {
            country.deathRate -= (baseStatValue + valueIncrement * level);
        }

        return true;
    }

    public override string GetDescription(object obj)
    {
        Country country = obj as Country;

        int level = PlayerPrefs.GetInt("Upgrades_" + name, 0);

        if (upgradeTarget == PopulationUpgradeType.MatingRate ||
            upgradeTarget == PopulationUpgradeType.BirthRate ||
            upgradeTarget == PopulationUpgradeType.DeathRate)
        {
            return "Current increase: +" + (baseStatValue + 100 * valueIncrement * level).ToString() + "%";
        }
        else if(upgradeTarget == PopulationUpgradeType.MaxCapacity)
        {
            return "Current increase: +" + (baseStatValue * level).ToString();
        }

        return "";
    }
}