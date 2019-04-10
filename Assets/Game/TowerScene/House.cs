using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class House
{    
    public Animal animal;

    public int houseObjectId = -1;

    [NonSerialized]
    public HouseObject houseObject;

    public void SetHouseObject(HouseObject houseObject)
    {
        houseObjectId = houseObject.id;
        this.houseObject = houseObject;
    }

    public void Load()
    {
        houseObject = GameManager.instance.gamedatabaseManager.GetObject<HouseObject>(houseObjectId);

        if(animal != null)
        {
            animal.Load();
        }
    }

    public void Save()
    {
        if(houseObject != null)
        {
            houseObjectId = houseObject.id;
        }
    }
}
