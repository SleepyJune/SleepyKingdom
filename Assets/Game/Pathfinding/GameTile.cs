using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GameTile
{
    [NonSerialized]
    public HashSet<GameTile> neighbours;

    public Vector3Int position;

    public double gScore;
    public double fScore;

    public GameTile parent = null;

    public GameTile(Vector3Int pos)
    {
        this.position = pos;
    }

    public double Distance(GameTile b)
    {
        return position.OffsetDistance(b.position);
    }
}