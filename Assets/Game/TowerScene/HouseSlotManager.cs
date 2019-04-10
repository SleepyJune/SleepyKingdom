using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TowerScene
{
    public class HouseSlotManager : MonoBehaviour
    {
        public HouseLevelObject[] houseLevelObjects;

        public Transform[] houseLevelParents;

        public HouseSlot houseSlotPrefab;

        public HousePopupController popupPrefab;
                
        List<HouseSlot> houseSlots = new List<HouseSlot>();
        
        [NonSerialized]
        public bool isShowingHouse = false;

        private void Start()
        {
            for (int i = 0; i < houseLevelObjects.Length; i++)
            {
                var houseLevelObject = houseLevelObjects[i];
                var houseLevelParent = houseLevelParents[i];

                for (int j = 0; j < houseLevelObject.numHouses; j++)
                {
                    var newHouse = Instantiate(houseSlotPrefab, houseLevelParent);
                    newHouse.levelIndex = i;
                    newHouse.houseIndex = j;
                    AddHouseSlot(newHouse);
                }
            }
        }

        public void ToggleShowHouse(bool show)
        {
            var maxHouseLevel = GameManager.instance.gameStateManager.gameState.houseLevels.Length;

            foreach (var slot in houseSlots)
            {
                if (show)
                {
                    if(slot.levelIndex < maxHouseLevel)
                    {
                        slot.ShowHouse();
                    }
                }
                else
                {
                    slot.HideHouse();
                }
            }

            isShowingHouse = show;
        }

        public void AddHouseSlot(HouseSlot slot)
        {
            var gameState = GameManager.instance.gameStateManager.gameState;

            if(slot.levelIndex < gameState.houseLevels.Length)
            {
                var level = gameState.houseLevels[slot.levelIndex];

                if(slot.houseIndex < level.houses.Length)
                {
                    var house = gameState.houseLevels[slot.levelIndex].houses[slot.houseIndex];

                    slot.SetHouse(house);                    
                }
            }

            houseSlots.Add(slot);
        }

        public void SetAddHouseSlot(HouseSlot houseSlot)
        {
            var newPopup = Instantiate(popupPrefab, TowerManager.instance.popupParent);
            newPopup.SetItem(houseSlot);
        }
    }
}
