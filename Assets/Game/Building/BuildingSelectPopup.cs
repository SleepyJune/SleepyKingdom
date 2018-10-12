using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BuildingSelectPopup : Popup
{
    [NonSerialized]
    public TowerSlotController slot;

    public Transform buildingList;

    public BuildingSelectItem itemPrefab;

    //private List<BuildingSelectItem> buildings = new List<BuildingSelectItem>();

    private void Start()
    {
        foreach(var building in GameManager.instance.gamedatabaseManager.buildingObjects.Values)
        {
            var newItem = Instantiate(itemPrefab, buildingList);

            newItem.SetBuilding(building, this);

            //buildings.Add(newItem);
        }
    }

    public void ShowPopup(TowerSlotController slot)
    {
        this.slot = slot;

        Show();
    }
}