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

    [NonSerialized]
    public Vector3Int targetPos;

    [NonSerialized]
    public Vector3Int[] path;

    [NonSerialized]
    public float startMovingTime;

    public bool canMove = true;

    public float speed = 1.0f;

    protected Tilemap tilemap;

    [NonSerialized]
    public MapUnitManager unitManager;

    [NonSerialized]
    public Territory territory;

    public bool rotateMovingUnit = false;
    public bool negativeRotation = true;

    protected virtual void Start()
    {
        MapSceneManager.instance.unitManager.InitializeUnit(this);

        tilemap = Pathfinder.tilemap;

        SetPosition(position);
    }
    
    public virtual bool SetMovePosition(Vector3Int pos)
    {                
        var tempPath = Pathfinder.GetShortestPath(this, position, pos, true);

        if(tempPath != null && tempPath.Length > 1)
        {
            path = tempPath;

            targetPos = path.Last();

            startMovingTime = Time.time;

            SetGameTilePosition(targetPos);

            return true;
        }

        return false;
    }

    void GetPosition()
    {
        var movingTime = Time.time - startMovingTime;
        var distance = movingTime * speed;

        var nextPosIndex = (int)Math.Ceiling(distance);
        var currentPosIndex = (int)Math.Floor(distance);

        if(path.Length >= nextPosIndex + 1)
        {
            var currentPos = tilemap.CellToWorld(path[currentPosIndex]);
            var nextPos = tilemap.CellToWorld(path[nextPosIndex]);
            
            var dir = (nextPos - currentPos).normalized;
            var distanceBetween = Vector3.Distance(currentPos, nextPos);

            var distLeft = distance - (float)Math.Truncate(distance);

            var pos = currentPos + dir * distanceBetween * distLeft;

            transform.position = pos;
            
            if (rotateMovingUnit && dir != Vector3.zero)
            {
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
                                
                if(angle > 180)
                {
                    angle -= 360;
                }
                else if(angle < -180)
                {
                    angle += 360;
                }

                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                if (!negativeRotation)
                {
                    var scale = transform.localScale;
                    var mult = angle < 0 ? -1 : 1;

                    Debug.Log(angle);

                    transform.localScale = new Vector3(mult * Math.Abs(scale.x), scale.y, scale.z);
                }

            }

            OnPositionChanged(position, path[currentPosIndex]);

            position = path[currentPosIndex];
        }
        else
        {
            var lastPos = path.Last();
            var currentPos = tilemap.CellToWorld(lastPos);

            transform.position = currentPos;

            OnPositionChanged(position, lastPos);

            position = lastPos;

            path = null;

            OnDestinationReached();
        }
    }

    protected virtual void Update()
    {
        if(canMove && path != null && path.Length > 1)
        {
            GetPosition();
        }
    }

    protected virtual void OnPositionChanged(Vector3Int oldPos, Vector3Int newPos)
    {

    }

    protected virtual void OnDestinationReached()
    {

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

    public void SetPosition(Vector3Int nextPosition)
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

    public float Distance(MapUnit unit)
    {
        return Vector3.Distance(position, unit.position);
    }
}