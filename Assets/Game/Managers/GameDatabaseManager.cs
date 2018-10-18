using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GameDatabaseManager : MonoBehaviour
{
    public GameDatabase database;

    public Dictionary<string, SpriteObject> spriteObjects = new Dictionary<string, SpriteObject>();

    public Dictionary<string, BuildingObject> buildingObjects = new Dictionary<string, BuildingObject>();

    public Dictionary<string, CastleObject> castleObjects = new Dictionary<string, CastleObject>();

    private void Awake()
    {
        foreach(var obj in database.allObjects)
        {
            if(obj is SpriteObject)
            {                
                spriteObjects.Add(obj.name, obj as SpriteObject);
            }

            if (obj is BuildingObject)
            {
                buildingObjects.Add(obj.name, obj as BuildingObject);
            }

            if (obj is CastleObject)
            {
                castleObjects.Add(obj.name, obj as CastleObject);
            }
        }

        /*foreach (var obj in database.allBuildings)
        {
            buildingObjects.Add(obj.buildingName, obj);
        }

        foreach (var obj in database.allCastles)
        {
            castleObjects.Add(obj.castleName, obj);
        }*/
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