using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "Game/Map Database")]
public class MapDatabase : ScriptableObject
{
    //public MapDataObject[] allObjects = new MapDataObject[0];

    public CountryDataObject[] countryDataObjects = new CountryDataObject[0];
    public CastleSpawnTile[] castleSpawnTiles = new CastleSpawnTile[0];

    public Dictionary<int, CountryDataObject> countryDataObjectDictionary = new Dictionary<int, CountryDataObject>();
    public Dictionary<Vector3Int, CastleSpawnTile> castleSpawnTileDictionary = new Dictionary<Vector3Int, CastleSpawnTile>();

    public int countryCounter = 0;

    public void InitDictionary()
    {
        MakeCountryDictionary();
        MakeCastleDictionary();
    }

    public void Save()
    {
        countryDataObjects = countryDataObjectDictionary.Values.ToArray();
        castleSpawnTiles = castleSpawnTileDictionary.Values.ToArray();
    }

    void MakeCountryDictionary()
    {
        Dictionary<int, CountryDataObject> ret = new Dictionary<int, CountryDataObject>();
        foreach (var data in countryDataObjects)
        {
            ret.Add(data.country.countryID, data);
            countryCounter = Math.Max(countryCounter, data.country.countryID);
        }

        countryDataObjectDictionary = ret;
        countryCounter += 1;
    }

    void MakeCastleDictionary()
    {
        Dictionary<Vector3Int, CastleSpawnTile> ret = new Dictionary<Vector3Int, CastleSpawnTile>();
        foreach (var data in castleSpawnTiles)
        {
            ret.Add(data.position, data);
        }

        castleSpawnTileDictionary = ret;
    }

}