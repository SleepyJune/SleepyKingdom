using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace TowerScene
{
    public class HouseInspectPopup : Popup
    {
        House currentSelectedHouse;

        public Image houseImage;
        public Image animalImage;

        public void SetHouse(House house)
        {
            currentSelectedHouse = house;

            if (house != null)
            {

                if (house.houseObject)
                {
                    houseImage.sprite = house.houseObject.image;
                }

                Show();
            }

        }
    }
}
