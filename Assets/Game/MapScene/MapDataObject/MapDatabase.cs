using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "Game/Map Database")]
public class MapDatabase : ScriptableObject
{
    public MapDatabaseObject[] mapDatabaseObjects = new MapDatabaseObject[0];

    public Dictionary<string, MapDatabaseObject> mapDatabaseDictionary = new Dictionary<string, MapDatabaseObject>();

    public void InitDictionary()
    {
        MakeMapDatabaseDictionary();
    }

    public void MakeMapDatabaseDictionary()
    {
        Dictionary<string, MapDatabaseObject> ret = new Dictionary<string, MapDatabaseObject>();
        foreach (var data in mapDatabaseObjects)
        {
            ret.Add(data.name, data);
        }

        mapDatabaseDictionary = ret;
    }

    public void Save()
    {
        mapDatabaseObjects = mapDatabaseDictionary.Values.ToArray();
    }
}