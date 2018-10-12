using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Building
{
    public int buildingID;

    public BuildingObject buildingObject;

    public static int buildingCounter;

    public static int GetBuildingCounter()
    {
        return buildingCounter++;
    }

    public static Building GenerateBuilding(BuildingObject obj)
    {
        var newBuilding = new Building();

        newBuilding.buildingID = GetBuildingCounter();
        newBuilding.buildingObject = obj;

        return newBuilding;
    }
}