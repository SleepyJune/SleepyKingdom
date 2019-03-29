using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class HouseItem : MonoBehaviour
{
    public Image itemIcon;
    public Text itemName;

    public Text itemDescription;

    HouseObject houseObject;

    HousePopupManager housePopupManager;

    public void SetItem(HouseObject item, HousePopupManager manager)
    {
        houseObject = item;

        this.housePopupManager = manager;

        RefreshItem();
    }

    private void RefreshItem()
    {
        itemIcon.sprite = houseObject.image;
        itemName.text = houseObject.name;
        itemDescription.text = houseObject.shortDescription;
    }

    public void OnButtonPressed()
    {
        housePopupManager.SetHouse(houseObject);
    }
}