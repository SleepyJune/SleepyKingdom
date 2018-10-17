using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

public class MapSceneManager : MonoBehaviour
{
    public Tilemap terrainMap;
    public Tilemap overlayMap;

    [NonSerialized]
    public UnitManager unitManager;

    public CastleWindowController castleWindow;

    private void Start()
    {
        unitManager = GetComponent<UnitManager>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            var position = terrainMap.WorldToCell(mousePos);

            var gameTile = Pathfinder.GetGameTile(position);

            if (gameTile != null && gameTile.units.Count > 0)
            {
                foreach(var unit in gameTile.units)
                {
                    if (unit is CastleUnit)
                    {
                        var castle = unit as CastleUnit;
                        castleWindow.SetCountry(castle.country);

                        break;
                    }
                }
            }
        }
    }

}