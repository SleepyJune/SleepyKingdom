using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "Game/GameDatabase")]

public class GameDatabase : ScriptableObject
{
    public SpriteObject[] allSprites = new SpriteObject[0];

    public BuildingObject[] allBuildings = new BuildingObject[0];
}