using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class HouseLevel
{
    public House[] houses;

    [NonSerialized]
    public List<House> housesList = new List<House>();

    public void Initialize()
    {
        foreach (var house in houses)
        {
            housesList.Add(house);
            house.Load();
        }
    }
}
