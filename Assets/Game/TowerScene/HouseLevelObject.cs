using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "Game/Tower/HouseLevel Object")]
public class HouseLevelObject : SpriteObject
{
    //public string description;

    public int numHouses;

    public int minHouseLevel = 1;
    public int maxHouseLevel = 1;    
}