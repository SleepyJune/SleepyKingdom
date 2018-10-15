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
    public Tilemap unitMap;

    [NonSerialized]
    public Tilemap overlayMap;

    public TileBase overlayPrefab;

    public Dictionary<Vector3Int, GameTile> map;

    private void Start()
    {
        //Invoke("Initialize", .05f);
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

                Debug.Log(tile.position.ToString());
            }

        }
    }

    public void Initialize()
    {
        var terrainGO = GameObject.Find("TerrainMap");
        var unitGO = GameObject.Find("UnitMap");
        var overlayGO = GameObject.Find("OverlayMap");

        if (terrainGO != null)
        {
            terrainMap = terrainGO.GetComponentInChildren<Tilemap>();
            unitMap = unitGO.GetComponentInChildren<Tilemap>();
            overlayMap = overlayGO.GetComponentInChildren<Tilemap>();

            InitializeMap();
            InitializeNeighbours();
        }
    }

    public void InitializeMap()
    {
        if (map == null)
        {
            map = new Dictionary<Vector3Int, GameTile>();
        }

        BoundsInt bounds = terrainMap.cellBounds;
        //TileBase[] allTiles = terrainMap.GetTilesBlock(bounds);

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);

                //TileBase tile = allTiles[x + y * bounds.size.x];

                var tile = terrainMap.GetTile(pos);

                if (tile != null)
                {
                    var newTile = new GameTile(new Vector3Int(x, y, 0));

                    if (!map.ContainsKey(newTile.position))
                    {
                        //Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
                        map.Add(newTile.position, newTile);
                    }
                }
            }
        }
    }

    public void InitializeNeighbours()
    {
        /*List<Vector3Int> directions = new List<Vector3Int>{
            new Vector3Int( 1, 0, 0), new Vector3Int( 1,-1, 0), new Vector3Int( 0,-1, 0),
            new Vector3Int(-1, 0, 0), new Vector3Int(-1, 1, 0), new Vector3Int( 0, 1, 0),
        };*/

        var directions = new int[][,] {
            new int[,]{{+1, 0}, {0, -1}, {-1, -1},{-1, 0}, {-1, +1}, {0, +1}},
            new int[,]{{+1,  0}, {+1, -1}, { 0, -1},{-1,  0}, { 0, +1}, {+1, +1}}
        };

        foreach (var tile in map.Values)
        {
            tile.neighbours = new HashSet<GameTile>();

            var parity = tile.position.x & 1;

            for (int i = 0; i < 6; i++)
            {
                var dir = new Vector3Int(directions[parity][i, 0], directions[parity][i, 1], 0);
                var pos = tile.position + dir;

                GameTile neighbour;
                if (map.TryGetValue(pos, out neighbour))
                {
                    tile.neighbours.Add(neighbour);

                    //Debug.Log("neighbour: " + neighbour.position.ToString());
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
}