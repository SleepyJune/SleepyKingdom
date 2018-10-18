﻿using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "Game/Building Object")]
public class BuildingObject : GameDataObject
{
    public SpriteObject spriteObject;

    public int cost;
}