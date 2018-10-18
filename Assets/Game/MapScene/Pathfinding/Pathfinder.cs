using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinder
{
    public static Dictionary<Vector3Int, GameTile> map = new Dictionary<Vector3Int, GameTile>();

    public static bool isInitialized = false;

    public static Tilemap tilemap;

    public static void Initialize(Tilemap tilemap)
    {
        if (!isInitialized)
        {
            InitializeMap(tilemap);
            InitializeNeighbours();
        }
    }

    public static void InitializeMap(Tilemap tilemap)
    {
        BoundsInt bounds = tilemap.cellBounds;

        Pathfinder.tilemap = tilemap;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);

                var tile = tilemap.GetTile(pos);

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

    public static void InitializeNeighbours()
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

    public static void ResetGameTile()
    {
        foreach (var sqr in map.Values)
        {
            sqr.gScore = 9999;
            sqr.fScore = 9999;
            sqr.parent = null;
        }
    }

    public static Vector3Int[] GetShortestPath(Unit unit, GameTile start, GameTile end)
    {
        HashSet<GameTile> closedSet = new HashSet<GameTile>();
        HashSet<GameTile> openSet = new HashSet<GameTile>();

        GameTile closestSquare = null;
        double closestDistance = 9999;

        openSet.Add(start);

        ResetGameTile();

        start.gScore = 0;
        start.fScore = start.Distance(end);

        double bestRouteDist = start.Distance(end);
                
        while (openSet.Count > 0)
        {
            var current = openSet.OrderBy(n => n.fScore).FirstOrDefault();

            if (current == end)
            {
                return GenerateWaypoints(start, current);
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (var neighbour in current.neighbours)
            {
                //var neighbourInfo = pair.Value;
                //var neighbour = neighbourInfo.neighbour;
                var distance = 1;// neighbourInfo.distance;


                if (closedSet.Contains(neighbour))
                {
                    continue;
                }

                /*if (!CanWalkToSquare(unit, neighbourInfo))
                {
                    continue;
                }*/

                var alternativeDistance = current.gScore + distance;

                if (!openSet.Contains(neighbour))
                {
                    openSet.Add(neighbour);
                }
                else if (alternativeDistance >= neighbour.gScore)
                {
                    continue;
                }

                var estimatedDistance = neighbour.Distance(end);
                                
                neighbour.parent = current;
                neighbour.gScore = alternativeDistance;
                neighbour.fScore = alternativeDistance + estimatedDistance;

                if (closestDistance > estimatedDistance)
                {
                    closestSquare = neighbour;
                    closestDistance = estimatedDistance;
                }

            }
        }

        /*if (closestSquare == null)
        {
            return null;
        }

        if (closestSquare.Distance(end) < start.Distance(end))
        {
            var path = PathInfo.GenerateWaypoints(start, closestSquare);
            path.reachable = false;

            return path;
        }
        else
        {
            return null;
        }*/

        return null;
    }

    public static Vector3Int[] GenerateWaypoints(GameTile start, GameTile end)
    {
        //var newPath = new PathInfo(start.pos, end.pos);

        List<Vector3Int> points = new List<Vector3Int>();

        var current = end;

        var lastSqr = end;
        Vector3 dir = Vector3.zero;

        while (current != null)
        {
            points.Add(current.position);

            lastSqr = current;
            current = current.parent;
        }

        points.Reverse();

        return points.ToArray();
    }

    public static GameTile GetGameTile(Vector3 pos)
    {
        return GetGameTile(tilemap.WorldToCell(pos));
    }

    public static GameTile GetGameTile(Vector3Int pos)
    {
        GameTile tile;
        if (map.TryGetValue(pos, out tile))
        {
            return tile;
        }

        return null;
    }
}