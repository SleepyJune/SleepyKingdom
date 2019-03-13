using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class PortalPopup : InteractablePopup
{
    public string mapName;

    public Text text;

    private void Start()
    {
        text.text = "Move to " + mapName + "?";
    }

    public void OnConfirmButtonPress()
    {
        unit.unitManager.worldMapManager.ChangeMap(mapName);
    }
}
