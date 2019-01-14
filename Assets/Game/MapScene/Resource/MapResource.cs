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

    public int amount;

    public int maxCapacity;

    MapResourceManager manager;

    [NonSerialized]
    public MapResourceObject resourceObject;

    public int resourceID = -1;

    Country myCountry;
            
    public void SetItem(MapResourceObject resourceObject, int amount, int max, Vector3Int pos, MapResourceManager manager)
    {
        canMove = false;

        this.resourceObject = resourceObject;
        this.amount = amount;
        this.maxCapacity = max;
        this.manager = manager;

        icon.sprite = resourceObject.image;
        resourceType = resourceObject.resourceType;

        resourceID = resourceObject.id;

        myCountry = myCountry = GameManager.instance.globalCountryManager.myCountry;

        position = pos;

        manager.resources.Add(this);
    }

    public override void Death()
    {
        manager.resources.Remove(this);
        base.Death();
    }

    public int CollectResource(int num)
    {
        if(amount <= 0)
        {
            return 0;
        }

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
            return num;
        }
        else
        {
            var collected = amount;
            amount -= amount;

            Death();
            return collected;
        }
    }

    public override void OnClickEvent()
    {
        manager.OnSelectResource(this);
    }

    public MapResourceSave Save()
    {
        MapResourceSave save = new MapResourceSave()
        {
            position = position,
            amount = amount,
            maxCapacity = maxCapacity,
            resourceID = resourceID,
        };

        return save;
    }
        
    public void OnAfterDeserialize()
    {
        if (resourceID != -1 && GameManager.instance)
        {
            resourceObject = GameManager.instance.gamedatabaseManager.GetObject<MapResourceObject>(resourceID);

            if (resourceObject == null)
            {
                Destroy(gameObject);
            }
            else
            {
                
            }
        }
    }
}