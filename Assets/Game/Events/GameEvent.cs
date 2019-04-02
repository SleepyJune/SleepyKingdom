using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GameEvent
{
    public string eventName;

    public Sprite image;

    public string shortDescription;

    public GameEvent(SpriteObject eventObject)
    {
        SetValues(eventObject);
    }

    public void SetValues(SpriteObject spriteObject)
    {
        this.eventName = spriteObject.name;
        this.image = spriteObject.image;
    }
}