using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MapResource : MapUnit
{
    public enum MapResourceType
    {
        Water,
        Wheat,
        Stone,
        Wood,
    }

    public MapResourceType resourceType;

    public SpriteRenderer icon;

    [NonSerialized]
    public int amount;

    [NonSerialized]
    public int maxCapacity;

    MapResourceManager manager;

    [NonSerialized]
    public MapResourceObject resourceObject;

    Country myCountry;
            
    public void SetItem(MapResourceObject resourceObject, int amount, MapResourceManager manager)
    {
        canMove = false;

        this.resourceObject = resourceObject;
        this.amount = amount;
        this.maxCapacity = amount;
        this.manager = manager;

        icon.sprite = resourceObject.image;
        resourceType = resourceObject.resourceType;
        
        myCountry = myCountry = GameManager.instance.globalCountryManager.myCountry;

        var castle = unitManager.myCastle;
        if(castle != null)
        {
            var dist = UnityEngine.Random.Range(1, 5);
            var angle = UnityEngine.Random.Range(0, 360);
            var dir = VectorExtensions.DegreeToVector(angle);

            var pos = castle.position + dir * dist;

            position = Pathfinder.tilemap.WorldToCell(pos);
        }

    }

    public int CollectResource(int num)
    {
        if(amount - num >= 0)
        {
            switch (resourceType)
            {
                case MapResourceType.Stone:
                    myCountry.stone += num;
                    break;
                case MapResourceType.Water:
                    myCountry.water += num;
                    break;
                case MapResourceType.Wheat:
                    myCountry.wheat += num;
                    break;
                case MapResourceType.Wood:
                    myCountry.wood += num;
                    break;
                default:
                    break;
            }            

            amount -= num;
            return amount;
        }
        else
        {
            Death();
            return 0;
        }
    }

    public override void OnMouseDownEvent()
    {
        manager.OnSelectResource(this);
    }
}