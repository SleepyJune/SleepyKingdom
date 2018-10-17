using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GameTile
{
    [NonSerialized]
    public HashSet<GameTile> neighbours;

    [NonSerialized]
    public HashSet<Unit> units;

    public Vector3Int position;

    public double gScore;
    public double fScore;

    public GameTile parent = null;

    public GameTile(Vector3Int pos)
    {
        this.position = pos;

        units = new HashSet<Unit>();
    }

    public double Distance(GameTile b)
    {
        return position.OffsetDistance(b.position);
    }

    public void AddUnit(Unit unit)
    {
        if (!units.Contains(unit))
        {
            units.Add(unit);
        }
    }

    public void DeleteUnit(Unit unit)
    {
        units.Remove(unit);
    }
}