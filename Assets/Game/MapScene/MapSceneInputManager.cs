using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

public class MapSceneInputManager : MonoBehaviour
{
    public delegate void OnGameTileClickDelegate(GameTile tile);
    public event OnGameTileClickDelegate OnGameTileClickEvent;

    MapSceneCameraController cameraController;

    TouchInputManager inputManager;

    int mapUnitMask;

    private void Start()
    {
        cameraController = GetComponent<MapSceneCameraController>();

        inputManager = GetComponent<TouchInputManager>();
        inputManager.touchStart += TouchStart;
        inputManager.touchMove += TouchMove;
        inputManager.touchEnd += TouchEnd;

        mapUnitMask = LayerMask.GetMask("Unit");
    }

    private void OnDestroy()
    {
        if (inputManager)
        {
            inputManager.touchStart -= TouchStart;
            inputManager.touchMove -= TouchMove;
            inputManager.touchEnd -= TouchEnd;
        }
    }

    public bool isMultiTouch()
    {
        return inputManager.isMultiTouch();
    }

    private void TouchStart(TouchInput input)
    {

    }

    private void TouchMove(TouchInput input)
    {
        if (inputManager.isMultiTouch())
        {

        }
        else
        {
            if (!input.isUIInteraction && input.isDrag)
            {
                cameraController.OnMouseDragEvent(input);
            }
        }
    }

    private void TouchEnd(TouchInput input)
    {
        ClickUnit(input);
    }

    private void ClickUnit(TouchInput input)
    {
        if (!input.isUIInteraction && !input.isDrag)
        {
            var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var mouse2D = new Vector2(worldPos.x, worldPos.y);
                        
            //var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            var hit = Physics2D.Raycast(mouse2D, Vector2.zero, 100);
            if (hit.collider != null && hit.collider.transform.parent != null)
            {
                var unit = hit.collider.transform.parent.GetComponent<MapUnit>();
                if(unit != null)
                {
                    unit.OnClickEvent();
                }
            }

            ClickTile(input);
        }
    }

    private void ClickTile(TouchInput input)
    {
        var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var tilePos = Pathfinder.tilemap.WorldToCell(worldPos);

        var gameTile = Pathfinder.GetGameTile(tilePos);

        if (gameTile != null)
        {
            if(OnGameTileClickEvent != null)
            {
                OnGameTileClickEvent(gameTile);
            }
        }
    }
}
