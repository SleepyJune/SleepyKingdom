using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class MapResourceManager : MonoBehaviour
{
    [NonSerialized]
    public MapUnitManager unitManager;

    public MapResource resourcePrefab;

    public Transform unitParent;

    public MapResourcePopup collectResourcePopup;

    [NonSerialized]
    public MapResource currentResource;

    [NonSerialized]
    public List<MapResource> resources = new List<MapResource>();

    float lastUpdateTime;

    List<MapResourceObject> resourceList;
        
    private void Start()
    {
        resourceList = GameManager.instance.gamedatabaseManager.mapResourcesObjects.Values.ToList();

        unitManager = GetComponent<MapUnitManager>();

        //Initialize();
    }

    private void Initialize()
    {
        foreach (var save in GameManager.instance.gameStateManager.gameState.mapResources)
        {
            Load(save);
        }
    }

    private void Update()
    {
        if (Time.time - lastUpdateTime > 1)
        {
            SpawnResource();
            lastUpdateTime = Time.time;
        }
    }

    void SpawnResource()
    {
        var castle = unitManager.myShip;
        if (castle == null)
        {
            return;
        }

        int tries = 0;
        while (resources.Count < 5 && tries < 50)
        {
            var rand = UnityEngine.Random.Range(0, Pathfinder.map.Count);

            var gameTile = Pathfinder.map.ElementAt(rand).Value;

            if (gameTile.mapResourceSpawn != null)
            {
                var amount = UnityEngine.Random.Range(100, 5000);
                var posInt = gameTile.position;

                if (!resources.Any(resource => resource.position == posInt))
                {
                    var newResource = Instantiate(resourcePrefab, unitParent);
                    newResource.SetItem(gameTile.mapResourceSpawn, amount, amount, posInt, this);
                    resources.Add(newResource);
                }
            }            

            tries += 1;
        }
    }

    void Load(MapResourceSave save)
    {
        var resourceObject = GameManager.instance.gamedatabaseManager.GetObject<MapResourceObject>(save.resourceID);

        if (resourceObject != null && save.amount > 0)
        {
            var newResource = Instantiate(resourcePrefab, unitParent);
            
            newResource.SetItem(resourceObject, save.amount, save.maxCapacity, save.position, this);
        }
    }

    public void Unload()
    {
        foreach(var resource in resources)
        {
            Destroy(resource.gameObject);
        }

        resources = new List<MapResource>();
    }

    void CollectResource()
    {
        if (currentResource != null)
        {
            var amountLeft = currentResource.CollectResource(100);
            collectResourcePopup.SetAmount(amountLeft);
            collectResourcePopup.ResetFill();
        }
    }

    public void OnSelectResource(MapResource resource)
    {
        currentResource = resource;
        
        collectResourcePopup.SetResource(resource);
    }

    public void OnCollectPressed()
    {
        //collect by button press or timed collect
    }

    public void StopCollecting()
    {
        currentResource = null;
    }
}