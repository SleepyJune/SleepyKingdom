using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MapInteractableUnit : MapUnit
{
    public Sprite image;

    public InteractablePopup popup;

    public override void OnClickEvent()
    {
        if(popup != null)
        {
            var newPopup = Instantiate(popup, unitManager.popupParent);
            newPopup.unit = this;
            newPopup.icon.sprite = image;
        }
    }
}
