using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class MapCastleUnit : MapUnit
{
    [NonSerialized]
    public Country country;

    [NonSerialized]
    public CastleObject castleObject;

    public SpriteRenderer castleSprite;

    protected override void Start()
    {
        base.Start();

        if(castleObject != null)
        {
            castleSprite.sprite = castleObject.image;
        }
    }

    protected override void OnDestinationReached()
    {
        country.position = position;
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

        castleObject = country.castleObject;
    }
}