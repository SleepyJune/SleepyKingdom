using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class MapResourcePopup : Popup
{
    public Image icon;
    public Text amount;

    public void SetResource(MapResource resource)
    {
        icon.sprite = resource.resourceObject.image;
        amount.text = resource.amount.ToString();

        Show();
    }
}