using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class MapCastleUnit : MapUnit
{
    [NonSerialized]
    public Country country;

    public Sprite image;

    public SpriteRenderer render;

    protected override void Start()
    {
        base.Start();

        if(render != null)
        {
            render.sprite = image;
        }
    }

    public override bool SetMovePosition(Vector3Int pos)
    {
        var hasPath = base.SetMovePosition(pos);

        if (this == unitManager.myCastle)
        {
            var worldPos = tilemap.CellToWorld(pos);

            if (hasPath)
            {
                if (unitManager.actionBar.myDestinationFlag != null)
                {
                    Destroy(unitManager.actionBar.myDestinationFlag);
                }
                                
                unitManager.actionBar.myDestinationFlag = Instantiate(unitManager.actionBar.flagPrefab, MapSceneManager.instance.overlayMap.transform);
                unitManager.actionBar.myDestinationFlag.transform.position = worldPos;
            }
            else
            {
                var xMark = Instantiate(unitManager.actionBar.xMarkPrefab, MapSceneManager.instance.overlayMap.transform);
                xMark.transform.position = worldPos;
            }
        }

        return hasPath;
    }

    protected override void OnDestinationReached()
    {
        country.position = position;


        if (this == unitManager.myCastle)
        {
            if (unitManager.actionBar.myDestinationFlag != null)
            {
                Destroy(unitManager.actionBar.myDestinationFlag);
            }
        }
    }

    public override void OnClickEvent()
    {
        if (!unitManager.inputManager.isMultiTouch())
        {
            unitManager.actionBar.OnCastleClick(this);
        }
    }

    public MapCastleSave Save()
    {
        MapCastleSave save = new MapCastleSave
        {
            countryID = country.countryID,
            position = country.position,
        };

        return save;
    }

    public void Load(MapCastleSave save, Country country)
    {
        position = save.position;

        this.country = country;
    }
}