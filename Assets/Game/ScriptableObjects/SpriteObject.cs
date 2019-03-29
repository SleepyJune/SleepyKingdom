using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Serialization;

public enum SpriteObjectType
{
    Weather,
    Resource,
    Building,
    Castle,
    ShopItem,
    Upgrade,
    Food,
    Unit,
    House,
}

[CreateAssetMenu(menuName = "Game/Sprite Object")]
public class SpriteObject : GameDataObject
{
    public SpriteObjectType spriteType;      
    
    public Sprite image;

    public string shortDescription;
}