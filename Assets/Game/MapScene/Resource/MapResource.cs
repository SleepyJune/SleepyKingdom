using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MapResource : MonoBehaviour
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

    MapResourceManager manager;

    [NonSerialized]
    public MapResourceObject resourceObject;

    Country myCountry;
        
    public void SetItem(MapResourceObject resourceObject, int amount, MapResourceManager manager)
    {
        this.resourceObject = resourceObject;
        this.amount = amount;
        this.manager = manager;

        icon.sprite = resourceObject.image;
        resourceType = resourceObject.resourceType;
        
        myCountry = myCountry = GameManager.instance.globalCountryManager.myCountry;
    }

    public void CollectResource(int num)
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
        }
        else
        {
            Death();
        }
    }

    public void Death()
    {
        Destroy(gameObject);
    }

    public void OnButtonClick()
    {
        manager.OnSelectResource(this);
    }
}