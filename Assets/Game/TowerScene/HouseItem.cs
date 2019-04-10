using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace TowerScene
{
    public class HouseItem : MonoBehaviour
    {
        public Image itemIcon;
        public Text itemName;

        public Text itemDescription;

        HouseObject houseObject;

        HousePopupController controller;

        public void SetItem(HouseObject item, HousePopupController controller)
        {
            houseObject = item;

            this.controller = controller;

            RefreshItem();
        }

        private void RefreshItem()
        {
            itemIcon.sprite = houseObject.image;
            itemName.text = houseObject.name;
        }

        public void OnButtonPressed()
        {
            controller.SetHouse(houseObject);
        }
    }
}