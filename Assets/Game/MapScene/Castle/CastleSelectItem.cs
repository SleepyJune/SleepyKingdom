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

    private CastleSelectPopup parent;

    public void SetBuilding(CastleObject target, CastleSelectPopup parent)
    {
        castle = target;

        icon.sprite = castle.image;
        buildingName.text = castle.name;

        this.parent = parent;
    }

    public void OnPress()
    {
        parent.ShowCreateCountryPopup(castle);
    }

}