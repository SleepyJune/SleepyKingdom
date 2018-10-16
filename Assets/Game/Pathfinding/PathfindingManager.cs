using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

public class PathfindingManager : MonoBehaviour
{
    [NonSerialized]
    public Tilemap terrainMap;

    [NonSerialized]
    public Tilemap overlayMap;

    public TileBase overlayPrefab;

    public Unit castle;

    public Dictionary<Vector3Int, GameTile> map;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            var testPos = terrainMap.WorldToCell(mousePos);
            var tile = GetGameTile(testPos);

            if (tile != null)
            {
                overlayMap.ClearAllTiles();
                overlayMap.SetTile(testPos, overlayPrefab);

                var start = GetGameTile(terrainMap.WorldToCell(castle.transform.position));
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

    public void Initialize()
    {
        var terrainGO = GameObject.Find("TerrainMap");
        //var unitGO = GameObject.Find("UnitMap");
        var overlayGO = GameObject.Find("OverlayMap");

        var castleGO = GameObject.Find("DirtCity");
        if (castleGO != null)
        {
            castle = castleGO.GetComponent<Unit>();
        }

        if (terrainGO != null)
        {
            terrainMap = terrainGO.GetComponentInChildren<Tilemap>();
            //unitMap = unitGO.GetComponentInChildren<Tilemap>();
            overlayMap = overlayGO.GetComponentInChildren<Tilemap>();

            InitializeMap();
            InitializeNeighbours();
        }
    }

    public void InitializeMap()
    {
        if (map == null)
        {
            map = Pathfinder.map;
        }

        BoundsInt bounds = terrainMap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);

                var tile = terrainMap.GetTile(pos);

                if (tile != null)
                {
                    var newTile = new GameTile(new Vector3Int(x, y, 0));

                    if (!map.ContainsKey(newTile.position))
                    {
                        map.Add(newTile.position, newTile);
                    }
                }
            }
        }
    }

    public void InitializeNeighbours()
    {
        var directions = HexVectorExtensions.hexDirections;

        foreach (var tile in map.Values)
        {
            tile.neighbours = new HashSet<GameTile>();

            var parity = tile.position.y & 1;

            for (int i = 0; i < 6; i++)
            {
                var dir = new Vector3Int(directions[parity][i, 0], directions[parity][i, 1], 0);
                var pos = new Vector3Int(tile.position.x + directions[parity][i, 0], tile.position.y + directions[parity][i, 1], 0);
                
                GameTile neighbour;
                if (map.TryGetValue(pos, out neighbour))
                {
                    tile.neighbours.Add(neighbour);
                }
            }
        }
    }

    public GameTile GetGameTile(Vector3Int pos)
    {
        GameTile tile;
        if (map.TryGetValue(pos, out tile))
        {
            return tile;
        }

        return null;
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