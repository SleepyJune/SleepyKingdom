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
    public MapResource currentCollecting;

    [NonSerialized]
    public List<MapResource> resources = new List<MapResource>();

    float lastUpdateTime;

    List<MapResourceObject> resourceList;

    [NonSerialized]
    public bool isCollecting = false;

    float lastCollectTime;

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

        if(Time.time - lastCollectTime > 1)
        {
            CollectResource();
            lastCollectTime = Time.time;
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
        foreach (var resource in resources)
        {
            if(resource && resource.gameObject)
            {
                Destroy(resource.gameObject);
            }
        }

        resources = new List<MapResource>();
    }

    void CollectResource()
    {
        if (isCollecting)
        {
            if (currentCollecting != null && currentCollecting.isCloseToShip())
            {
                var amountCollected = currentCollecting.CollectResource(100);
                collectResourcePopup.SetAmount(amountCollected);
                collectResourcePopup.ResetFill();
                
                if(currentCollecting.amount <= 0)
                {
                    StopCollecting();
                    collectResourcePopup.Hide();
                }
            }
            else
            {
                isCollecting = false;
            }
        }        
    }

    public void StartCollecting(MapResource resource)
    {
        currentCollecting = resource;
        currentResource = resource;
        isCollecting = true;
        collectResourcePopup.Show();
    }

    public void OnShipMoved()
    {
        StopCollecting();
    }

    public void OnSelectResource(MapResource resource)
    {
        currentResource = resource;

        collectResourcePopup.SetResource(resource);
    }

    public void OnCollectPressed()
    {
        //collect by button press or timed collect
        if (currentResource != null)
        {
            unitManager.actionBar.SetTarget(currentResource);
            collectResourcePopup.Hide();
        }
    }

    public void StopCollecting()
    {
        isCollecting = false;
        currentCollecting = null;
    }
}