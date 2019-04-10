using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace TowerScene
{
    public class HouseSlot : MonoBehaviour
    {
        public Image houseIcon;
        public Image animalIcon;

        [NonSerialized]
        public int levelIndex = -1;
        [NonSerialized]
        public int houseIndex = -1;

        [NonSerialized]
        public House house;

        HouseSlotManager houseManager;
        HouseInspectPopup houseInspectPopup;

        CanvasGroup canvasGroup;

        private void Start()
        {
            houseManager = TowerManager.instance.houseSlotManager;

            houseInspectPopup = TowerManager.instance.houseInspectPopup;

            canvasGroup = GetComponent<CanvasGroup>();

            houseManager.AddHouseSlot(this);

            HideHouse();
            HideAnimal();
        }

        public void HideAnimal()
        {
            if(house == null || house.animal == null || house.animal.animalUnitId == -1)
            {
                animalIcon.gameObject.SetActive(false);
            }
            else
            {
                animalIcon.gameObject.SetActive(true);
            }
        }

        public virtual void HideHouse()
        {
            if (house == null || house.houseObjectId == -1)
            {
                canvasGroup.alpha = 0;
                canvasGroup.interactable = false;
            }
            else
            {
                canvasGroup.alpha = 1;
                canvasGroup.interactable = true;
            }
        }

        public void ShowHouse()
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
        }

        public void SetHouse(House house)
        {
            this.house = house;

            if (house.houseObject)
            {
                SetHouseObject(house.houseObject);
            }
                
            //unity sets uninstantiated prefab as null?
            if (house.animal != null && house.animal.animalUnitId != -1)
            {
                animalIcon.sprite = house.animal.animalUnit.faceImage;
                HideAnimal();
            }
        }

        public void SetHouseObject(HouseObject houseObject)
        {
            if (house == null)
            {
                house = new House();
            }

            house.SetHouseObject(houseObject);
            houseIcon.sprite = house.houseObject.image;
        }

        public void OnButtonPress()
        {
            if (house == null || house.houseObjectId == -1)
            {
                houseManager.SetAddHouseSlot(this);
            }
            else
            {
                houseInspectPopup.SetHouse(house);
            }
        }
    }
}
