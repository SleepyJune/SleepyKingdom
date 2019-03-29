using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class HousePopupManager : MonoBehaviour
{
    public Popup housePopup;
    public Transform houseItemParent;

    public HouseItem houseItemPrefab;

    [NonSerialized]
    public HouseSlot currentSelectedSlot;

    public GameObject closeButton;

    private void Start()
    {
        Transform lastItem = null;

        foreach(var item in GameManager.instance.gamedatabaseManager.spriteObjects.Values)
        {
            if(item is HouseObject)
            {
                var houseObject = item as HouseObject;

                var newHouse = Instantiate(houseItemPrefab, houseItemParent);
                newHouse.SetItem(houseObject, this);

                lastItem = newHouse.transform;
            }
        }

        if(lastItem != null)
        {
            closeButton.transform.SetSiblingIndex(lastItem.GetSiblingIndex() + 1);
        }
    }

    public void SetItem(HouseSlot cage)
    {
        currentSelectedSlot = cage;
        housePopup.Show();
    }

    public void SetHouse(HouseObject houseObject)
    {
        housePopup.Hide();

        if (currentSelectedSlot)
        {
            currentSelectedSlot.SetHouseObject(houseObject);
        }
    }
}
