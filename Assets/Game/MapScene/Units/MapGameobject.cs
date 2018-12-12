using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class MapGameobject : MonoBehaviour
{
    public Vector3Int position;

    public GameTile gameTile;

    private Tilemap tilemap;
    
    protected virtual void Start()
    {
        tilemap = Pathfinder.tilemap;

        SetPosition(position);
    }
    
    public virtual void SetPosition(Vector3Int nextPosition)
    {
        position = nextPosition;

        transform.position = tilemap.CellToWorld(position);
    }
}