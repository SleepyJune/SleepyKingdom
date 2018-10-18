using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MapDevMode : MonoBehaviour
{
    public CastleSelectPopup createCastlePrefab;

    [NonSerialized]
    public Transform canvas;

    private void Start()
    {
        var canvasGO = GameObject.Find("Canvas");
        if(canvasGO != null)
        {
            canvas = canvasGO.transform;
        }
    }

    private void Update()
    {
        CheckCreateCastle();
    }

    public void CheckCreateCastle()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            var tile = Pathfinder.GetGameTile(mousePos);

            if (tile != null)
            {
                CreateCastleMenu(tile.position);
            }

        }
    }

    public void CreateCastleMenu(Vector3Int position)
    {
        if(canvas != null)
        {
            var newPopup = Instantiate(createCastlePrefab, canvas);
            newPopup.selectedPosition = position;

        }
    }
}