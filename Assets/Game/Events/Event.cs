using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Event
{
    public string eventName;

    public Sprite image;

    public GameEventType eventType;

    public string shortDescription;

    public Event(EventObject eventObject)
    {
        SetValues(eventObject);
    }

    public void SetValues(EventObject eventObject)
    {
        this.eventName = eventObject.name;
        this.image = eventObject.image;
        this.eventType = eventObject.eventType;
        this.shortDescription = eventObject.shortDescription;
    }
}