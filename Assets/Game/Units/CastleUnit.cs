using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CastleUnit : Unit
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
}