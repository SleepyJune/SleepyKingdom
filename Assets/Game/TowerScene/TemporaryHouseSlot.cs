using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TowerScene
{
    public class TemporaryHouseSlot : MonoBehaviour
    {
        House house;

        HouseSlotManager houseManager;
        HouseInspectPopup houseInspectPopup;

        CanvasGroup canvasGroup;

        private void Start()
        {
            houseManager = TowerManager.instance.houseSlotManager;

            houseInspectPopup = TowerManager.instance.houseInspectPopup;

            //houseManager.AddHouseSlot(this);
        }
    }
}
