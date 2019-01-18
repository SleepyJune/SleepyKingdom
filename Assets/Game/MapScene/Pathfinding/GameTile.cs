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
    
    public GameTile(Vector3Int pos, bool isBlocked = false)
    {
        this.position = pos;
        this.isBlocked = isBlocked;

        units = new HashSet<MapUnit>();
    }

    public double Distance(GameTile b)
    {
        return position.OffsetDistance(b.position);
    }

    public bool isUnitBlocked()
    {
        return units.Count > 0;
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