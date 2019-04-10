using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TowerScene
{
    public class HousePopupController : Popup
    {
        public Transform houseItemParent;

        public HouseItem houseItemPrefab;

        [NonSerialized]
        public HouseSlot currentSelectedSlot;

        public GameObject closeButton;

        private void Start()
        {
            Transform lastItem = null;

            foreach (var item in GameManager.instance.gamedatabaseManager.spriteObjects.Values)
            {
                if (item is HouseObject)
                {
                    var houseObject = item as HouseObject;

                    var newHouse = Instantiate(houseItemPrefab, houseItemParent);
                    newHouse.SetItem(houseObject, this);

                    lastItem = newHouse.transform;
                }
            }

            if (lastItem != null)
            {
                closeButton.transform.SetSiblingIndex(lastItem.GetSiblingIndex() + 1);
            }
        }

        public void SetItem(HouseSlot house)
        {
            currentSelectedSlot = house;
            Show();
        }

        public void SetHouse(HouseObject houseObject)
        {
            if (currentSelectedSlot)
            {
                currentSelectedSlot.SetHouseObject(houseObject);

                TowerManager.instance.houseSlotManager.ToggleShowHouse(false);
                TowerManager.instance.sideUIController.HideCancelButton();
            }

            Close();
        }
    }
}

