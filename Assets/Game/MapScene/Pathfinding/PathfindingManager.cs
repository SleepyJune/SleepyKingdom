using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

public class PathfindingManager : MonoBehaviour
{
    private MapSceneManager mapSceneManager;

    private Tilemap terrainMap;
    private Tilemap overlayMap;

    public TileBase overlayPrefab;

    public MapUnit castle;

    public Dictionary<Vector3Int, GameTile> map;

    private void Start()
    {
        mapSceneManager = GetComponent<MapSceneManager>();
        terrainMap = mapSceneManager.terrainMap;
        overlayMap = mapSceneManager.overlayMap;
        
        Pathfinder.Initialize(mapSceneManager.terrainMap);

        map = Pathfinder.map;
    }

    private void Update()
    {
        return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            var testPos = terrainMap.WorldToCell(mousePos);
            var tile = Pathfinder.GetGameTile(testPos);

            if (tile != null)
            {
                overlayMap.ClearAllTiles();
                overlayMap.SetTile(testPos, overlayPrefab);

                var start = Pathfinder.GetGameTile(terrainMap.WorldToCell(castle.transform.position));
                castle.path = Pathfinder.GetShortestPath(castle, start, tile);
                castle.startMovingTime = Time.time;

                /*foreach(var path in castle.path)
                {
                    overlayMap.SetTile(path, overlayPrefab);
                }*/

                /*foreach (var neighbour in tile.neighbours)
                {
                    overlayMap.SetTile(neighbour.position, overlayPrefab);
                }*/

                //castle.path = new Vector3Int[] { testPos };
            }

        }
    }



    public Vector3 CellToWorldPosition(Vector3 pos)
    {
        return terrainMap.LocalToWorld(terrainMap.CellToLocalInterpolated(pos));
    }

    public Vector3 CellToGridWorldPosition(Vector3 pos)
    {
        return terrainMap.CellToWorld(CellToCellPosition(pos));
    }

    public Vector3Int CellToCellPosition(Vector3 pos)
    {
        return terrainMap.LocalToCell(terrainMap.CellToLocalInterpolated(pos));
    }
}