using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MapAnimalManager : MonoBehaviour
{
    [NonSerialized]
    public List<AnimalUnit> animals = new List<AnimalUnit>();

    private Dictionary<AnimalSpawnTile, int> animalsSpawned = new Dictionary<AnimalSpawnTile, int>();

    public AnimalCapturePopup capturePopupPrefab;

    MapUnitManager unitManager;

    private void Start()
    {
        unitManager = GetComponent<MapUnitManager>();

        Initialize();
    }

    public void Update()
    {
        //Dictionary<AnimalSpawnTile, int> toBeUpdated = new Dictionary<AnimalSpawnTile, int>();

        foreach (var pair in animalsSpawned.ToList())
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
                        newAnimal.spawnTile = spawnTile;

                        animals.Add(newAnimal);

                        unitManager.AddUnit(newAnimal);

                        animalsSpawned[spawnTile] = numSpawned + 1;
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