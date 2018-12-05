using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class MapResourceManager : MonoBehaviour
{
    public MapResource resourcePrefab;

    public Transform unitParent;

    public MapResourcePopup collectResourcePopup;

    [NonSerialized]
    public MapResource currentResource;

    bool isCollecting = false;

    List<MapResource> resources = new List<MapResource>();

    float lastUpdateTime;

    List<MapResourceObject> resourceList;

    private void Start()
    {
        resourceList = GameManager.instance.gamedatabaseManager.mapResourcesObjects.Values.ToList();
    }

    private void Update()
    {
        if(Time.time >= lastUpdateTime)
        {
            SpawnResource();
            CollectResource();
            lastUpdateTime = Time.time + 1;
        }
    }

    void SpawnResource()
    {
        if(resources.Count < 5)
        {
            //var newResource = 
            int index = UnityEngine.Random.Range(0, resourceList.Count);
            var newResource = Instantiate(resourcePrefab, unitParent);

            newResource.SetItem(resourceList[index], 5000, this);

            resources.Add(newResource);
        }
    }

    void CollectResource()
    {
        if(isCollecting && currentResource != null)
        {
            currentResource.CollectResource(100);
        }
    }

    public void OnSelectResource(MapResource resource)
    {
        currentResource = resource;

        collectResourcePopup.SetResource(resource);
    }

    public void OnCollectPressed()
    {
        isCollecting = true;
    }

    public void StopCollecting()
    {
        currentResource = null;
        isCollecting = false;
    }
}