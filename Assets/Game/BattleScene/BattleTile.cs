using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BattleTile : MonoBehaviour
{
    [NonSerialized]
    public HashSet<BattleTile> neighbours;

    [NonSerialized]
    public HashSet<BattleUnit> units;

    public Vector3Int position;

    public double gScore;
    public double fScore;

    public BattleTile parent = null;

    public BattleTile(Vector3Int pos)
    {
        this.position = pos;

        units = new HashSet<BattleUnit>();
    }

    public void AddUnit(BattleUnit unit)
    {
        if (!units.Contains(unit))
        {
            units.Add(unit);
        }
    }

    public void DeleteUnit(BattleUnit unit)
    {
        units.Remove(unit);
    }
}