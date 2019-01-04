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

    public MapResourceCollector mapCollectorPrefab;

    [NonSerialized]
    public MapResource currentResource;

    [NonSerialized]
    public List<MapResource> resources = new List<MapResource>();

    float lastUpdateTime;

    List<MapResourceObject> resourceList;

    Dictionary<MapResource, MapResourceCollector> collectors = new Dictionary<MapResource, MapResourceCollector>();

    private void Start()
    {
        resourceList = GameManager.instance.gamedatabaseManager.mapResourcesObjects.Values.ToList();

        unitManager = GetComponent<MapUnitManager>();

        Initialize();
    }

    private void Initialize()
    {
        foreach (var save in GameManager.instance.gameStateManager.gameState.mapResources)
        {
            //unitManager.InitializeUnit(unit);
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
        var castle = unitManager.myCastle;
        if (castle == null)
        {
            return;
        }

        int tries = 0;
        while (resources.Count < 5 && tries < 50)
        {
            var dist = UnityEngine.Random.Range(1, 5);
            var angle = UnityEngine.Random.Range(0, 360);
            var dir = VectorExtensions.DegreeToVector(angle);

            var pos = castle.position + dir * dist;

            int index = UnityEngine.Random.Range(0, resourceList.Count);

            var amount = UnityEngine.Random.Range(100, 5000);

            var posInt = Pathfinder.tilemap.WorldToCell(pos);

            if (!resources.Any(resource => resource.position == posInt))
            {
                var newResource = Instantiate(resourcePrefab, unitParent);
                newResource.SetItem(resourceList[index], amount, amount, posInt, this);
            }

            tries += 1;
        }
    }

    void Load(MapResourceSave save)
    {
        var resourceObject = GameManager.instance.gamedatabaseManager.GetObject(save.resourceID) as MapResourceObject;

        if (resourceObject != null)
        {
            var newResource = Instantiate(resourcePrefab, unitParent);
            unitManager.InitializeUnit(newResource);

            newResource.SetItem(resourceObject, save.amount, save.maxCapacity, save.position, this);
        }
    }

    void CollectResource()
    {
        if (currentResource != null && !collectors.ContainsKey(currentResource))
        {
            var amountLeft = currentResource.CollectResource(100);
            collectResourcePopup.SetAmount(amountLeft);
            collectResourcePopup.ResetFill();
        }
    }

    public void OnSelectResource(MapResource resource)
    {
        currentResource = resource;

        MapResourceCollector collector = null;
        collectors.TryGetValue(resource, out collector);

        collectResourcePopup.SetResource(resource, collector);
    }

    public void OnCollectPressed()
    {
        if (currentResource != null && !collectors.ContainsKey(currentResource))
        {
            var newCollector = Instantiate(mapCollectorPrefab, unitParent);
            unitManager.InitializeUnit(newCollector);
            newCollector.SetResource(currentResource);
        }
    }

    public void StopCollecting()
    {
        currentResource = null;
    }
}