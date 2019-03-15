using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class MapMobileUnit : MapUnit
{
    [NonSerialized]
    public Vector3Int targetPos;

    [NonSerialized]
    public Vector3Int[] path;

    [NonSerialized]
    public float startMovingTime;

    public bool canMove = true;

    public float speed = 1.0f;

    public bool rotateMovingUnit = false;
    public bool negativeRotation = true;

    public bool isMoving = false;

    public virtual bool SetMovePosition(Vector3Int pos)
    {
        var tempPath = Pathfinder.GetShortestPath(this, position, pos, true);

        if (tempPath != null && tempPath.Length > 1)
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

        if (path.Length >= nextPosIndex + 1)
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

                if (angle > 180)
                {
                    angle -= 360;
                }
                else if (angle < -180)
                {
                    angle += 360;
                }

                var oldRot = transform.rotation;                

                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                OnRotationChanged(oldRot, transform.rotation);

                if (!negativeRotation)
                {
                    var scale = transform.localScale;
                    var mult = angle < 0 ? -1 : 1;

                    transform.localScale = new Vector3(mult * Math.Abs(scale.x), scale.y, scale.z);
                }

            }

            isMoving = true;

            var oldPos = position;

            position = path[currentPosIndex];

            OnPositionChanged(oldPos, position);
        }
        else
        {
            var lastPos = path.Last();
            var currentPos = tilemap.CellToWorld(lastPos);

            transform.position = currentPos;

            Vector3Int oldPos = position;
            
            position = lastPos;

            OnPositionChanged(oldPos, position);
            
            path = null;

            isMoving = false;

            OnDestinationReached();
        }
    }

    protected virtual void Update()
    {
        if (canMove && path != null && path.Length > 1)
        {
            GetPosition();
        }
    }

    public override void SetPosition(Vector3Int nextPosition)
    {
        Vector3Int oldPos = position;

        base.SetPosition(nextPosition);

        OnPositionChanged(oldPos, position);
    }

    protected virtual void OnPositionChanged(Vector3Int oldPos, Vector3Int newPos)
    {

    }

    protected virtual void OnRotationChanged(Quaternion oldRot, Quaternion newRot)
    {

    }

    protected virtual void OnDestinationReached()
    {

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
}