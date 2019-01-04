using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using System.Linq;

public class GameDatabaseManager : MonoBehaviour
{
    public GameDatabase database;

    public Dictionary<int, GameDataObject> allObjects = new Dictionary<int, GameDataObject>();

    public Dictionary<int, SpriteObject> spriteObjects = new Dictionary<int, SpriteObject>();

    public Dictionary<int, BuildingObject> buildingObjects = new Dictionary<int, BuildingObject>();

    public Dictionary<int, CastleObject> castleObjects = new Dictionary<int, CastleObject>();

    public Dictionary<int, ShopItemObject> shopItemObjects = new Dictionary<int, ShopItemObject>();

    public Dictionary<int, CountryUpgradeObject> countryUpgradeObjects = new Dictionary<int, CountryUpgradeObject>();

    public Dictionary<int, BattleUnitObject> battleUnitObjects = new Dictionary<int, BattleUnitObject>();

    public Dictionary<int, MapResourceObject> mapResourcesObjects = new Dictionary<int, MapResourceObject>();

    private void Awake()
    {
        foreach(var obj in database.allObjects)
        {
            if(obj is SpriteObject)
            {                
                spriteObjects.Add(obj.id, obj as SpriteObject);
            }

            if (obj is BuildingObject)
            {
                buildingObjects.Add(obj.id, obj as BuildingObject);
            }

            if (obj is CastleObject)
            {
                castleObjects.Add(obj.id, obj as CastleObject);
            }

            if(obj is ShopItemObject)
            {
                shopItemObjects.Add(obj.id, obj as ShopItemObject);
            }

            if(obj is CountryUpgradeObject)
            {
                countryUpgradeObjects.Add(obj.id, obj as CountryUpgradeObject);
            }

            if(obj is BattleUnitObject)
            {
                battleUnitObjects.Add(obj.id, obj as BattleUnitObject);
            }

            if(obj is MapResourceObject)
            {
                mapResourcesObjects.Add(obj.id, obj as MapResourceObject);
            }

            allObjects.Add(obj.id, obj);
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

    public List<T> GetAllObjects<T>(int id) where T : GameDataObject
    {
        List<int> blah = new List<int>();
        
        return allObjects.Values.OfType<T>().ToList();
    }

    public GameDataObject GetObject(int id)
    {
        GameDataObject obj;
        if (allObjects.TryGetValue(id, out obj))
        {
            return obj;
        }

        return null;
    }

    public SpriteObject GetSpriteObject(int id)
    {
        SpriteObject obj;
        if(spriteObjects.TryGetValue(id, out obj))
        {
            return obj;
        }

        return null;
    }

    public BuildingObject GetBuildingObject(int id)
    {
        BuildingObject obj;
        if (buildingObjects.TryGetValue(id, out obj))
        {
            return obj;
        }

        return null;
    }
}