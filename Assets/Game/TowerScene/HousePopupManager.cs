using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class HousePopupManager : MonoBehaviour
{
    public Popup housePopup;
    public Transform houseItemParent;

    public HouseItem houseItemPrefab;

    public HouseSlot currentSelectedCageSlot;

    private void Start()
    {
        foreach(var item in GameManager.instance.gamedatabaseManager.spriteObjects.Values)
        {
            if(item is HouseObject)
            {
                var houseObject = item as HouseObject;

                var newHouse = Instantiate(houseItemPrefab, houseItemParent);
                newHouse.SetItem(houseObject, this);
            }
        }
    }

    public void SetItem(HouseSlot cage)
    {
        currentSelectedCageSlot = cage;
        housePopup.Show();
    }

    public void SetHouse(HouseObject houseObject)
    {
        housePopup.Hide();

        if (currentSelectedCageSlot)
        {
            currentSelectedCageSlot.SetHouseObject(houseObject);
        }
    }
}
