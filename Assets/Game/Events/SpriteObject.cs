using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Serialization;

public enum SpriteObjectType
{
    Weather,
    Resource,
}

[CreateAssetMenu(menuName = "Events/Event Object")]
public class SpriteObject : ScriptableObject
{
    public SpriteObjectType spriteType;
        
    public string spriteName;

    public Sprite image;

    public string shortDescription;
}