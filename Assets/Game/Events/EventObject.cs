using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "Events/Event Object")]
public class EventObject : ScriptableObject
{
    public string eventName;

    public Sprite image;

    public string shortDescription;
}