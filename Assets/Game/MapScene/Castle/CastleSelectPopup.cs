using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CastleSelectPopup : Popup
{
    public Transform buildingList;

    public CastleSelectItem itemPrefab;

    private void Start()
    {
        foreach (var castle in GameManager.instance.gamedatabaseManager.castleObjects.Values)
        {
            var newItem = Instantiate(itemPrefab, buildingList);

            newItem.SetBuilding(castle, this);

            //buildings.Add(newItem);
        }
    }

    public void ShowPopup(TowerSlotController slot)
    {
        //this.slot = slot;

        Show();
    }
}