using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GameTile
{
    [NonSerialized]
    public HashSet<GameTile> neighbours;

    public Vector3Int position;

    public GameTile(Vector3Int pos)
    {
        this.position = pos;
    }
}