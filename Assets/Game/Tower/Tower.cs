using System;
using System.Collections;
using System.Collections.Generic;

using Random = UnityEngine.Random;

using UnityEngine;

public class Tower
{
    public Country country;
    
    public TowerFloor[] floors = new TowerFloor[maxFloors];

    public static int maxFloors = 10;

    public static Tower Generate()
    {
        var newTower = new Tower();
        
        int randFloors = Random.Range(3, maxFloors);

        for (int i = 0; i < maxFloors; i++)
        {
            if (i <= randFloors)
            {
                newTower.floors[i] = TowerFloor.Generate(newTower, i);
            }
            else
            {
                newTower.floors[i] = null;
            }
        }

        return newTower;
    }
}