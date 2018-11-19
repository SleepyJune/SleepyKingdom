using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class UpgradeUIManager : MonoBehaviour
{
    public Transform upgradeList;

    public UpgradeItem upgradeItemPrefab;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var list = GameManager.instance.gamedatabaseManager.countryUpgradeObjects;

        foreach (var item in list.Values)
        {
            var newItem = Instantiate(upgradeItemPrefab, upgradeList);

            newItem.SetItem(item);
        }
    }
}