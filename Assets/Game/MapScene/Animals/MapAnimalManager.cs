using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MapAnimalManager : MonoBehaviour
{
    [NonSerialized]
    public List<AnimalUnit> animals = new List<AnimalUnit>();

    private Dictionary<AnimalSpawnTile, int> animalsSpawned = new Dictionary<AnimalSpawnTile, int>();

    MapUnitManager unitManager;

    private void Start()
    {
        unitManager = GetComponent<MapUnitManager>();

        Initialize();
    }

    public void Update()
    {
        foreach(var pair in animalsSpawned)
        {
            var spawnTile = pair.Key;
            var numSpawned = pair.Value;

            var animal = spawnTile.prefab;
            if(animal != null)
            {
                if(numSpawned < 1)
                {
                    var random = UnityEngine.Random.Range(0, 100);
                    if(random == 0)
                    {
                        var newAnimal = Instantiate(animal, unitManager.unitParent);
                        newAnimal.position = spawnTile.position;

                        animals.Add(newAnimal);
                    }
                }
            }
        }
    }

    public void Initialize()
    {
        Unload();

        animals = new List<AnimalUnit>();

        foreach (var save in GameManager.instance.gamedatabaseManager.currentMap.animalSpawnTileDictionary.Values)
        {
            animalsSpawned.Add(save, 0);
        }
    }

    public void Unload()
    {
        foreach (var animal in animals)
        {
            Destroy(animal.gameObject);
        }
    }
}