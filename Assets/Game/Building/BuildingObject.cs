using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "Game/Building/Building Object")]
public class BuildingObject : ScriptableObject
{
    public string buildingName;

    public SpriteObject spriteObject;

    public int cost;
}