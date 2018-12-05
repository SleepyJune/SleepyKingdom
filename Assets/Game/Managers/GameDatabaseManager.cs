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

    public Dictionary<string, ShopItemObject> shopItemObjects = new Dictionary<string, ShopItemObject>();

    public Dictionary<string, CountryUpgradeObject> countryUpgradeObjects = new Dictionary<string, CountryUpgradeObject>();

    public Dictionary<string, BattleUnitObject> battleUnitObjects = new Dictionary<string, BattleUnitObject>();

    public Dictionary<string, MapResourceObject> mapResourcesObjects = new Dictionary<string, MapResourceObject>();

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

            if(obj is ShopItemObject)
            {
                shopItemObjects.Add(obj.name, obj as ShopItemObject);
            }

            if(obj is CountryUpgradeObject)
            {
                countryUpgradeObjects.Add(obj.name, obj as CountryUpgradeObject);
            }

            if(obj is BattleUnitObject)
            {
                battleUnitObjects.Add(obj.name, obj as BattleUnitObject);
            }

            if(obj is MapResourceObject)
            {
                mapResourcesObjects.Add(obj.name, obj as MapResourceObject);
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