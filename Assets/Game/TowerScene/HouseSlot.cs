using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class HouseSlot : MonoBehaviour
{
    public Image houseIcon;

    HouseObject houseObject;

    HouseSlotManager cageManager;
    HousePopupManager housePopupManager;

    private void Start()
    {
        cageManager = TowerManager.instance.houseSlotManager;
        housePopupManager = TowerManager.instance.housePopupManager;

        cageManager.AddCageSlot(this);
    }

    public void SetHouseObject(HouseObject house)
    {
        houseObject = house;
        houseIcon.sprite = houseObject.image;
    }

    public void OnButtonPress()
    {
        if (houseObject == null)
        {
            housePopupManager.SetItem(this);
        }
    }
}
