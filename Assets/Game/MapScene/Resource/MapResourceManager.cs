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

    [NonSerialized]
    public MapResource currentResource;

    List<MapResource> resources = new List<MapResource>();

    float lastUpdateTime;

    List<MapResourceObject> resourceList;

    float collectionSpeed = 1;
    float nextCollectTime;

    Dictionary<MapResource, MapResourceCollector> collectors = new Dictionary<MapResource, MapResourceCollector>();

    private void Start()
    {
        resourceList = GameManager.instance.gamedatabaseManager.mapResourcesObjects.Values.ToList();
    }

    private void Update()
    {
        SpawnResource();

        if (Time.time >= nextCollectTime)
        {
            CollectResource();

            nextCollectTime = Time.time + (1 / collectionSpeed);
        }
    }

    void SpawnResource()
    {
        if(resources.Count < 5)
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

        collectResourcePopup.SetResource(resource, collectionSpeed);
    }

    public void OnCollectPressed()
    {
        
    }

    public void StopCollecting()
    {
        currentResource = null;
    }
}