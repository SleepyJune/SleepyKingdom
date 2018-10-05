using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SpriteObjectManager : MonoBehaviour
{
    public SpriteDatabase database;

    public Dictionary<string, SpriteObject> spriteObjects = new Dictionary<string, SpriteObject>();

    private void Awake()
    {
        foreach(var sprite in database.allSprites)
        {
            spriteObjects.Add(sprite.spriteName, sprite);
        }
    }
}