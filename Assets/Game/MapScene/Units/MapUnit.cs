using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class MapUnit : GameDataPrefab
{
    public int unitId;

    public Vector3Int position;

    public GameTile gameTile;

    public bool isBlocked = false;

    protected Tilemap tilemap;

    [NonSerialized]
    public MapUnitManager unitManager;

    protected virtual void Start()
    {
        MapSceneManager.instance.unitManager.AddUnit(this);

        tilemap = Pathfinder.tilemap;

        SetPosition(position);
    }
    

    public virtual void OnDragEvent()
    {

    }

    public virtual void OnClickEvent()
    {

    }

    public virtual void Death()
    {
        unitManager.allUnits.Remove(unitId);
        Destroy(gameObject);
    }

    public virtual void SetPosition(Vector3Int nextPosition)
    {
        position = nextPosition;

        transform.position = tilemap.CellToWorld(position);

        SetGameTilePosition(nextPosition);
    }

    void SetGameTilePosition(Vector3Int nextPosition)
    {
        if (gameTile != null)
        {
            gameTile.DeleteUnit(this);
        }

        var newGameTile = Pathfinder.GetGameTile(nextPosition);
        if (newGameTile != null)
        {
            newGameTile.AddUnit(this);
            gameTile = newGameTile;
        }
    }

    public bool isCloseToShip()
    {
        return isClose(unitManager.myShip);
    }

    public bool isClose(MapUnit unit)
    {
        var myGameTile = Pathfinder.GetGameTile(transform.position);
        var unitGameTile = Pathfinder.GetGameTile(unit.transform.position);

        if(myGameTile != null && unitGameTile != null)
        {
            return myGameTile == unitGameTile || myGameTile.neighbours.Contains(unitGameTile);
        }

        return false;
    }

    public float Distance(MapUnit unit)
    {
        return Vector3.Distance(position, unit.position);
    }
}