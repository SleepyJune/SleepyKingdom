using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class HouseSlot : MonoBehaviour
{
    public Image houseIcon;

    House house;
    
    HouseSlotManager cageManager;
    HousePopupManager housePopupManager;
    HouseInspectPopup houseInspectPopup;

    private void Start()
    {
        cageManager = TowerManager.instance.houseSlotManager;
        housePopupManager = TowerManager.instance.housePopupManager;

        houseInspectPopup = TowerManager.instance.houseInspectPopup;

        cageManager.AddCageSlot(this);
    }

    public void SetHouseObject(HouseObject houseObject)
    {
        if(house == null)
        {
            house = new House();
        }

        house.houseObject = houseObject;
        houseIcon.sprite = house.houseObject.image;
    }

    public void OnButtonPress()
    {
        if (house == null)
        {
            housePopupManager.SetItem(this);
        }
        else
        {
            houseInspectPopup.SetHouse(house);
        }
    }
}
