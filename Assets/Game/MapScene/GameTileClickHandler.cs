using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GameTileClickHandler : MonoBehaviour
{
    public delegate void GameTileClickedFn(GameTile tile);
    public event GameTileClickedFn OnGameTileClickedEvent;

    private void OnMouseDown()
    {
        var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var tilePos = Pathfinder.tilemap.WorldToCell(worldPos);

        var gameTile = Pathfinder.GetGameTile(tilePos);

        if(gameTile != null)
        {
            if(OnGameTileClickedEvent != null)
            {
                OnGameTileClickedEvent(gameTile);
            }
        }
    }
}