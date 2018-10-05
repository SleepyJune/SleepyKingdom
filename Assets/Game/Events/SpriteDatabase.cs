using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "Events/EventDatabase")]

public class SpriteDatabase : ScriptableObject
{
    public SpriteObject[] allSprites = new SpriteObject[0];
}