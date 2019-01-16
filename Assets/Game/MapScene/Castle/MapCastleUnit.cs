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

    protected override void Start()
    {
        base.Start();

        if(render != null)
        {
            render.sprite = image;
        }
    }

    protected override void OnDestinationReached()
    {
        country.position = position;
    }

    public override void OnClickEvent()
    {
        if (!unitManager.inputManager.isMultiTouch())
        {
            unitManager.actionBar.OnCastleClick(this);
        }
    }

    public MapCastleSave Save()
    {
        MapCastleSave save = new MapCastleSave
        {
            countryID = country.countryID,
            position = country.position,
        };

        return save;
    }

    public void Load(MapCastleSave save, Country country)
    {
        position = save.position;

        this.country = country;
    }
}