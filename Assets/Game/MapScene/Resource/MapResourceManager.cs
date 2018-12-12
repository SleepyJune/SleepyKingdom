using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class MapResourceManager : MonoBehaviour
{
    public MapUnitManager unitManager;
    public MapResource resourcePrefab;

    public Transform unitParent;

    public MapResourcePopup collectResourcePopup;

    public MapResourceCollector mapCollectorPrefab;

    [NonSerialized]
    public MapResource currentResource;

    List<MapResource> resources = new List<MapResource>();

    float lastUpdateTime;

    List<MapResourceObject> resourceList;

    Dictionary<MapResource, MapResourceCollector> collectors = new Dictionary<MapResource, MapResourceCollector>();

    private void Start()
    {
        resourceList = GameManager.instance.gamedatabaseManager.mapResourcesObjects.Values.ToList();
    }

    private void Update()
    {
        if(Time.time - lastUpdateTime > 1)
        {
            SpawnResource();
            lastUpdateTime = Time.time;
        }
    }

    void SpawnResource()
    {
        while(resources.Count < 5)
        {
            //var newResource = 
            int index = UnityEngine.Random.Range(0, resourceList.Count);
            var newResource = Instantiate(resourcePrefab, unitParent);

            unitManager.InitializeUnit(newResource);

            var amount = UnityEngine.Random.Range(100, 5000);

            newResource.SetItem(resourceList[index], amount, this);

            resources.Add(newResource);
        }
    }

    void CollectResource()
    {
        if(currentResource != null && !collectors.ContainsKey(currentResource))
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