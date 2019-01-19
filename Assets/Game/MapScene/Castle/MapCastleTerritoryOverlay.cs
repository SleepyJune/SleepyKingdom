using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

public class MapCastleTerritoryOverlay : MonoBehaviour
{
    public Tilemap territoryMap;

    public TerritoryTile greenTile;
    public TerritoryTile redTile;
        
    HashSet<MapCastleUnit> units = new HashSet<MapCastleUnit>();
    HashSet<Vector3Int> territoryPoints = new HashSet<Vector3Int>();

    private void Start()
    {
        territoryMap.gameObject.SetActive(true);
        territoryMap.ClearAllTiles();
    }

    public void CreateTerritoryOverlay(MapCastleUnit unit)
    {       
        if (!units.Contains(unit) && unit.territory != null)
        {
            var newSet = new HashSet<Vector3Int>(unit.territory.points);
            newSet.ExceptWith(territoryPoints);
                        
            var arr = new TerritoryTile[newSet.Count];
            for (int i = 0; i < arr.Length; i++) arr[i] = redTile;

            territoryMap.SetTiles(newSet.ToArray(), arr);
            territoryPoints.UnionWith(newSet);

            units.Add(unit);
        }
    }

    public void RemoveTerritoryOverlay(MapCastleUnit unit)
    {
        if(units.Contains(unit) && unit.territory != null)
        {
            var newSet = new HashSet<Vector3Int>(unit.territory.points);
            newSet.IntersectWith(territoryPoints);

            var arr = new TerritoryTile[newSet.Count];

            territoryMap.SetTiles(newSet.ToArray(), arr);
            territoryPoints.ExceptWith(newSet);

            units.Remove(unit);
        }
    }
}
