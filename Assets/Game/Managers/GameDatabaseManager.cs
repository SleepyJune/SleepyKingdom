using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using System.Linq;

public class GameDatabaseManager : MonoBehaviour
{
    public GameDatabase database;

    public MapDatabase mapDatabase;

    public Dictionary<int, GameDataObject> allObjects = new Dictionary<int, GameDataObject>();
    public Dictionary<int, GameDataPrefab> allPrefabs = new Dictionary<int, GameDataPrefab>();

    public Dictionary<int, SpriteObject> spriteObjects = new Dictionary<int, SpriteObject>();

    public Dictionary<int, CastleObject> castleObjects = new Dictionary<int, CastleObject>();

    public Dictionary<int, ShopItemObject> shopItemObjects = new Dictionary<int, ShopItemObject>();

    public Dictionary<int, CountryUpgradeObject> countryUpgradeObjects = new Dictionary<int, CountryUpgradeObject>();

    public Dictionary<int, MapResourceObject> mapResourcesObjects = new Dictionary<int, MapResourceObject>();

    private void Awake()
    {
        InitializeObjectDictionary();
        InitializePrefabDictionary();

        mapDatabase.InitDictionary();
    }

    void InitializePrefabDictionary()
    {
        foreach (var prefab in database.allPrefabs)
        {
            allPrefabs.Add(prefab.id, prefab);
        }
    }

    void InitializeObjectDictionary()
    {
        foreach (var obj in database.allObjects)
        {
            if (obj is SpriteObject)
            {
                spriteObjects.Add(obj.id, obj as SpriteObject);
            }
            
            if (obj is CastleObject)
            {
                castleObjects.Add(obj.id, obj as CastleObject);
            }

            if (obj is ShopItemObject)
            {
                shopItemObjects.Add(obj.id, obj as ShopItemObject);
            }

            if (obj is CountryUpgradeObject)
            {
                countryUpgradeObjects.Add(obj.id, obj as CountryUpgradeObject);
            }

            if (obj is MapResourceObject)
            {
                mapResourcesObjects.Add(obj.id, obj as MapResourceObject);
            }

            allObjects.Add(obj.id, obj);
        }
    }

    public List<T> GetAllObjects<T>(int id) where T : GameDataObject
    {
        List<int> blah = new List<int>();
        
        return allObjects.Values.OfType<T>().ToList();
    }

    public T GetPrefab<T>(int id) where T : GameDataPrefab
    {
        GameDataPrefab prefab;
        if (allPrefabs.TryGetValue(id, out prefab))
        {
            var ret = prefab as T;

            if(ret != null)
            {
                return ret;
            }
        }

        return null;
    }

    public T GetObject<T>(int id) where T : GameDataObject
    {
        GameDataObject obj;
        if (allObjects.TryGetValue(id, out obj))
        {
            var ret = obj as T;

            if (ret != null)
            {
                return ret;
            }
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
}