using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class Animal
{
    [NonSerialized]
    public AnimalUnit animalUnit;

    public int animalUnitId = -1;

    public int happiness = 10;

    public void Load()
    {
        animalUnit = GameManager.instance.gamedatabaseManager.GetPrefab<AnimalUnit>(animalUnitId);
    }

    public void Save()
    {
        if (animalUnit != null)
        {
            animalUnitId = animalUnit.id;
        }
    }
}
