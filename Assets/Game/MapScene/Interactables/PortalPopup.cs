using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class PortalPopup : InteractablePopup
{
    private string mapName;

    public Text text;

    MapPortalUnit portal;

    MapShip myShip;

    private void Start()
    {
        portal = unit as MapPortalUnit;

        mapName = portal.mapName;

        myShip = unit.unitManager.myShip;

        if(unit.isCloseToShip())
        {
            text.text = "Move to " + mapName + "?";
        }
        else
        {
            text.text = "Portal to " + mapName + ". Please move ship closer to use the portal.";
        }
    }

    public void OnConfirmButtonPress()
    {
        if (unit.isCloseToShip())
        {
            unit.unitManager.worldMapManager.ChangeMap(mapName);
        }
    }
}
