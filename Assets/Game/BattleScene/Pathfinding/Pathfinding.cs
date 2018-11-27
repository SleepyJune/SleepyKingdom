using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace BattlePathfinding
{
    public static class Pathfinding
    {
        public static Dictionary<Vector3Int, GameTile> map = new Dictionary<Vector3Int, GameTile>();

        public static bool isInitialized = false;
        
        public static void Initialize()
        {
            if (!isInitialized)
            {
                InitializeMap();
                InitializeNeighbours();
            }
        }

        public static void InitializeMap()
        {
            BoundsInt bounds = new BoundsInt(-20,-20,-1,40,40,2);

            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);

                    var newTile = new GameTile(new Vector3Int(x, y, 0));

                    if (!map.ContainsKey(newTile.position))
                    {
                        map.Add(newTile.position, newTile);
                    }
                }
            }
        }

        public static void InitializeNeighbours()
        {
            var directions = new Vector3Int[]
            {
                new Vector3Int(-1,-1,0),
                new Vector3Int(0,-1,0),
                new Vector3Int(1,-1,0),

                new Vector3Int(-1,0,0),
                new Vector3Int(0,0,0),
                new Vector3Int(1,0,0),

                new Vector3Int(-1,1,0),
                new Vector3Int(0,1,0),
                new Vector3Int(1,1,0),
            };


            foreach (var tile in map.Values)
            {
                tile.neighbours = new HashSet<GameTile>();
                
                for (int i = 0; i < directions.Length; i++)
                {
                    var pos = new Vector3Int(tile.position.x + directions[i].x, tile.position.y + directions[i].y, 0);

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

        public static Vector3Int[] GetShortestPath(MapUnit unit, Vector3Int start, Vector3Int end)
        {
            var startTile = GetGameTile(start);
            var endTile = GetGameTile(end);

            if (startTile != null && endTile != null)
            {
                return GetShortestPath(unit, startTile, endTile);
            }

            return null;
        }

        public static Vector3Int[] GetShortestPath(MapUnit unit, GameTile start, GameTile end)
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
            //return GetGameTile(tilemap.WorldToCell(pos));
            return null;
        }

        public static GameTile GetNode(Vector3Int pos)
        {
            GameTile tile;
            if (map.TryGetValue(pos, out tile))
            {
                return tile;
            }

            return null;
        }
    }
}