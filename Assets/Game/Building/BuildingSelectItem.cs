﻿using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class BuildingSelectItem : MonoBehaviour
{
    public Image icon;

    public Text buildingName;

    private BuildingObject building;

    private BuildingSelectPopup popup;

    public void SetBuilding(BuildingObject target, BuildingSelectPopup parent)
    {
        building = target;

        icon.sprite = building.spriteObject.image;
        buildingName.text = building.buildingName;

        popup = parent;
    }

    public void OnPress()
    {
        popup.slot.SetBuilding(new Building(building));
        popup.Close();
    }

}