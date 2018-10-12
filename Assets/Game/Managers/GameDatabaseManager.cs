using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GameDatabaseManager : MonoBehaviour
{
    public GameDatabase database;

    public Dictionary<string, SpriteObject> spriteObjects = new Dictionary<string, SpriteObject>();

    public Dictionary<string, BuildingObject> buildingObjects = new Dictionary<string, BuildingObject>();

    private void Awake()
    {
        foreach(var obj in database.allSprites)
        {
            spriteObjects.Add(obj.spriteName, obj);
        }

        foreach (var obj in database.allBuildings)
        {
            buildingObjects.Add(obj.buildingName, obj);
        }
    }

    public SpriteObject GetSpriteObject(string name)
    {
        SpriteObject obj;
        if(spriteObjects.TryGetValue(name, out obj))
        {
            return obj;
        }

        return null;
    }

    public BuildingObject GetBuildingObject(string name)
    {
        BuildingObject obj;
        if (buildingObjects.TryGetValue(name, out obj))
        {
            return obj;
        }

        return null;
    }
}