using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MapInteractableUnit : MapUnit
{
    public Sprite image;

    public InteractablePopup popup;

    public SpriteRenderer render;

    protected override void Start()
    {
        if (render != null)
        {
            render.sprite = image;
        }

        base.Start();
    }

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
