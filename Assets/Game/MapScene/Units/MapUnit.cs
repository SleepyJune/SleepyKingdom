using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class MapUnit : MonoBehaviour
{
    public Vector3Int position;

    public GameTile gameTile;

    [NonSerialized]
    public Vector3Int targetPos;

    [NonSerialized]
    public Vector3Int[] path;

    [NonSerialized]
    public float startMovingTime;

    public float speed = 1.0f;

    private Tilemap tilemap;

    [NonSerialized]
    public MapUnitManager unitManager;

    protected virtual void Start()
    {
        tilemap = Pathfinder.tilemap;

        SetPosition(position);
    }
    
    public virtual void OnMouseDownEvent()
    {
        unitManager.OnUnitMouseClickEvent(this);
    }

    public void SetMovePosition(Vector3Int pos)
    {                
        var tempPath = Pathfinder.GetShortestPath(this, position, pos);

        if(tempPath != null && tempPath.Length > 1)
        {
            path = tempPath;

            targetPos = pos;

            startMovingTime = Time.time;
        }
    }

    private void Update()
    {
        if(path != null && path.Length > 1)
        {
            var nextPos = path[1];

            var nextPosWorld = tilemap.CellToWorld(nextPos);
            //var positionWorld = GameManager.instance.pathfindingManager.CellToWorldPosition(position);

            var movingTime = Time.time - startMovingTime;

            if (Vector3.Distance(nextPosWorld, transform.position) >= .01f && movingTime * speed < 1f)
            {
                var previousPosWorld = tilemap.CellToWorld(path[0]);

                Vector3 dir = (nextPosWorld - previousPosWorld).normalized;

                transform.position = previousPosWorld + dir * speed * movingTime;

                //position += dir * speed * Time.deltaTime;


                //transform.position = GameManager.instance.pathfindingManager.CellToWorldPosition(position);
            }
            else
            {                
                path = path.Skip(1).ToArray();

                SetPosition(nextPos);

                startMovingTime = Time.time;
            }
        }
    }

    public void SetPosition(Vector3Int nextPosition)
    {
        position = nextPosition;

        transform.position = tilemap.CellToWorld(position);

        if (gameTile != null)
        {
            gameTile.DeleteUnit(this);
        }

        var newGameTile = Pathfinder.GetGameTile(nextPosition);
        if(newGameTile != null)
        {
            newGameTile.AddUnit(this);
            gameTile = newGameTile;
        }
    }
}