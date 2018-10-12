using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class TowerSlotController : MonoBehaviour
{
    [NonSerialized]
    public Building building;

    public Image iconImage;

    private Sprite emptySlotSprite;

    private BuildingSelectPopup popup;

    private void Start()
    {
        //emptySlotSprite = GameManager.instance.spriteObjectManager.GetSpriteObject("EmptySlot").image;

        emptySlotSprite = iconImage.sprite;

        popup = FindObjectOfType<BuildingSelectPopup>();
    }

    public void SetBuilding(Building newBuilding)
    {
        if(newBuilding.buildingID != 0)
        {
            building = newBuilding;
            iconImage.sprite = building.buildingObject.spriteObject.image;
        }
        else
        {
            building = null;
            iconImage.sprite = emptySlotSprite;
        }
    }

    public void OnIconPress()
    {
        popup.ShowPopup(this);
    }
}