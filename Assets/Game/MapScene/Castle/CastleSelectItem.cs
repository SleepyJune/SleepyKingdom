using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class CastleSelectItem : MonoBehaviour
{
    public Image icon;

    public Text buildingName;

    private CastleObject castle;

    private CastleSelectPopup popup;

    public void SetBuilding(CastleObject target, CastleSelectPopup parent)
    {
        castle = target;

        icon.sprite = castle.spriteObject.image;
        buildingName.text = castle.name;

        popup = parent;
    }

    public void OnPress()
    {
        //popup.slot.SetBuilding(new Building(castle));
        //popup.Close();
    }

}