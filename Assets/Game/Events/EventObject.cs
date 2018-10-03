using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public enum GameEventType
{
    Weather,
}

[CreateAssetMenu(menuName = "Events/Event Object")]
public class EventObject : ScriptableObject
{
    public GameEventType eventType;

    public string eventName;

    public Sprite image;

    public string shortDescription;
}