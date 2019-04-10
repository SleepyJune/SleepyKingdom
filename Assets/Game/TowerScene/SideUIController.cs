using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TowerScene
{
    public class SideUIController : MonoBehaviour
    {
        public GameObject cancelButton;

        public void OnAddHouseButtonPressed()
        {
            TowerManager.instance.houseSlotManager.ToggleShowHouse(true);
            cancelButton.SetActive(true);
        }

        public void OnAddLevelButtonPressed()
        {
            GameManager.instance.gameStateManager.gameState.AddHouseLevel();
            TowerManager.instance.houseSlotManager.ToggleShowHouse(true);
            cancelButton.SetActive(true);
        }

        public void OnCancelButtonPressed()
        {
            TowerManager.instance.houseSlotManager.ToggleShowHouse(false);
            cancelButton.SetActive(false);
        }

        public void HideCancelButton()
        {
            cancelButton.SetActive(false);
        }
    }
}
