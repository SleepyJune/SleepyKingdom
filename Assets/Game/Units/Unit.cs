using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class Unit : MonoBehaviour
{
    //public Vector3 position;
    
    public Vector3 targetPos;

    public Vector3Int[] path;

    public float startMovingTime;

    public float speed = .01f;

    private void Update()
    {
        if(path != null && path.Length > 1)
        {
            var tileMap = GameManager.instance.pathfindingManager.terrainMap;

            var nextPos = path[1];

            var nextPosWorld = tileMap.CellToWorld(nextPos);
            //var positionWorld = GameManager.instance.pathfindingManager.CellToWorldPosition(position);

            if (Vector3.Distance(nextPosWorld, transform.position) >= .01f)
            {
                var previousPosWorld = tileMap.CellToWorld(path[0]);

                Vector3 dir = (nextPosWorld - previousPosWorld).normalized;

                var movingTime = Time.time - startMovingTime;

                transform.position = previousPosWorld + dir * speed * movingTime;

                //position += dir * speed * Time.deltaTime;


                //transform.position = GameManager.instance.pathfindingManager.CellToWorldPosition(position);
            }
            else
            {
                transform.position = tileMap.CellToWorld(nextPos);
                path = path.Skip(1).ToArray();

                startMovingTime = Time.time;
            }
        }
    }

    /*public void UpdatePosition()
    {
        if (path != null && path.Length > 1)
        {
            var tileMap = GameManager.instance.pathfindingManager.terrainMap;

            var movingTime = Time.time - startMovingTime;
            

            var nextPos = path[1];

            var nextPosWorld = GameManager.instance.pathfindingManager.CellToWorldPosition(nextPos);
            var positionWorld = GameManager.instance.pathfindingManager.CellToWorldPosition(position);

            if (Vector3.Distance(nextPosWorld, positionWorld) >= .1f)
            {
                var previousPosWorld = tileMap.CellToWorld(path[0]);

                Vector3 dir = (nextPosWorld - previousPosWorld).normalized;

                transform.position = previousPosWorld + dir;

                //position += dir * speed * Time.deltaTime;


                //transform.position = GameManager.instance.pathfindingManager.CellToWorldPosition(position);
            }
            else
            {
                transform.position = GameManager.instance.pathfindingManager.CellToGridWorldPosition(position);
                path = path.Skip(1).ToArray();
            }
        }
    }*/
}