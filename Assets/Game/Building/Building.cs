using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Random = UnityEngine.Random;

[Serializable]
public class Building
{
    public int buildingID;

    public string buildingObjectName;

    public BuildingObject buildingObject;

    public static int buildingCounter = 1;

    public Building(BuildingObject obj)
    {
        buildingID = GetBuildingCounter();
        buildingObject = obj;
        buildingObjectName = obj.name;
    }

    public static int GetBuildingCounter()
    {
        return buildingCounter++;
    }

    public static Building GenerateBuilding()
    {
        var list = GameManager.instance.gamedatabaseManager.buildingObjects;

        var randomIndex = Random.Range(0, list.Count);

        BuildingObject obj = list.ElementAt(randomIndex).Value;

        var newBuilding = new Building(obj);

        return newBuilding;
    }
}