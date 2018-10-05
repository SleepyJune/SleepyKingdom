using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Event
{
    public string eventName;

    public Sprite image;

    public SpriteObjectType eventType;

    public string shortDescription;

    public Event(SpriteObject eventObject)
    {
        SetValues(eventObject);
    }

    public void SetValues(SpriteObject spriteObject)
    {
        this.eventName = spriteObject.name;
        this.image = spriteObject.image;
        this.eventType = spriteObject.spriteType;
        this.shortDescription = spriteObject.shortDescription;
    }
}