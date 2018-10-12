using System;
using System.Collections;
using System.Collections.Generic;

using Random = UnityEngine.Random;

using UnityEngine;

[Serializable]
public class TowerFloor
{
    public Tower tower;

    public int floorNum;

    public int floorSize;

    public Building[] buildings = new Building[maxBuildings];

    public static int maxBuildings = 6;

    public static int maxSize = 13;

    public TowerFloor(Tower tower, int floorNum, int size)
    {
        this.tower = tower;
        this.floorNum = floorNum;
        this.floorSize = size;
    }

    public static TowerFloor Generate(Tower tower, int floorNum)
    {
        int previousSize = maxSize;

        if(floorNum > 0)
        {
            var previousFloor = tower.floors[floorNum - 1];
            if(previousFloor != null)
            {
                previousSize = previousFloor.floorSize;
            }
        }

        //int randSize = Random.Range(3, Mathf.Min(previousSize, maxSize));

        var newFloor = new TowerFloor(tower, floorNum, previousSize-1);

        int randFloors = Random.Range(0, maxBuildings);

        for(int i=0;i< maxBuildings; i++)
        {
            if(i <= randFloors)
            {
                newFloor.buildings[i] = Building.GenerateBuilding();
            }
        }

        return newFloor;
    }
}