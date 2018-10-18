using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Tilemaps;

public class Unit : MonoBehaviour
{
    public Vector3Int position;

    public GameTile gameTile;

    [NonSerialized]
    public Vector3 targetPos;

    [NonSerialized]
    public Vector3Int[] path;

    [NonSerialized]
    public float startMovingTime;

    public float speed = 1.0f;

    private Tilemap tilemap;

    [NonSerialized]
    public UnitManager unitManager;

    protected virtual void Start()
    {
        tilemap = Pathfinder.tilemap;

        SetPosition(position);
    }

    public virtual void OnMouseDownEvent()
    {
        unitManager.OnUnitMouseClickEvent(this);
    }

    private void Update()
    {
        if(path != null && path.Length > 1)
        {
            var nextPos = path[1];

            var nextPosWorld = tilemap.CellToWorld(nextPos);
            //var positionWorld = GameManager.instance.pathfindingManager.CellToWorldPosition(position);

            if (Vector3.Distance(nextPosWorld, transform.position) >= .01f)
            {
                var previousPosWorld = tilemap.CellToWorld(path[0]);

                Vector3 dir = (nextPosWorld - previousPosWorld).normalized;

                var movingTime = Time.time - startMovingTime;

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