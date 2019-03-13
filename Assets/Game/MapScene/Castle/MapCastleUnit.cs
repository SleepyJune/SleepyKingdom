using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class MapCastleUnit : MapUnit
{
    [NonSerialized]
    public Country country;

    public Sprite image;

    public SpriteRenderer render;

    public bool isInsideTerritory = false;

    protected override void Start()
    {
        base.Start();

        if(render != null)
        {
            render.sprite = image;
        }
    }

    public override void OnClickEvent()
    {
        if (!unitManager.inputManager.isMultiTouch())
        {
            unitManager.actionBar.OnUnitClick(this);
        }
    }
}