using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GameTile
{
    [NonSerialized]
    public HashSet<GameTile> neighbours;

    [NonSerialized]
    public HashSet<MapUnit> units;

    public Vector3Int position;

    public double gScore;
    public double fScore;

    public GameTile parent = null;

    public bool isBlocked = false;

    public MapResourceObject mapResourceSpawn;
    
    public GameTile(Vector3Int pos, bool isBlocked = false, MapResourceObject mapResourceSpawn = null)
    {
        this.position = pos;
        this.isBlocked = isBlocked;
        this.mapResourceSpawn = mapResourceSpawn;

        units = new HashSet<MapUnit>();
    }

    public double Distance(GameTile b)
    {
        return position.OffsetDistance(b.position);
    }

    public bool isUnitBlocked()
    {
        //return units.Count > 0;
        foreach(var unit in units)
        {
            if (unit.isBlocked)
            {
                return true;
            }
        }

        return false;
    }

    public void AddUnit(MapUnit unit)
    {
        if (!units.Contains(unit))
        {
            units.Add(unit);
        }
    }

    public void DeleteUnit(MapUnit unit)
    {
        units.Remove(unit);
    }
}