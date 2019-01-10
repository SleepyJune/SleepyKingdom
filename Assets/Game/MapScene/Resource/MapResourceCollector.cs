using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MapResourceCollector : MapUnit
{
    public MapResource resource;

    MapCastleUnit castle;

    public GameObject resourceParent;
    public SpriteRenderer icon;

    Animator anim;

    bool isHarvesting = false;

    int amountCollected = 0;
    int maxCapacity = 1000;

    float harvestStartTime;

    float lastHarvestUpdateTime;

    protected override void Start()
    {
        anim = GetComponent<Animator>();
        base.Start();

        SetResource();
    }

    void SetResource()
    {
        castle = unitManager.myCastle;

        position = castle.position;
        SetMovePosition(resource.position);

        StartCollecting();
    }

    protected override void Update()
    {
        base.Update();

        if(Time.time - lastHarvestUpdateTime > 1f)
        {
            Harvest();
            lastHarvestUpdateTime = Time.time;
        }
    }

    protected override void OnDestinationReached()
    {
        if(resource != null && position == resource.position)
        {            
            StartHarvest();
        }
        else if(position == castle.position)
        {
            UnloadResources();

            if (resource != null && resource.amount > 0)
            {
                SetMovePosition(resource.position);
            }
        }
    }

    void UnloadResources()
    {
        if(amountCollected > 0)
        {
            amountCollected = 0;
        }

        resourceParent.SetActive(false);
    }

    void StartHarvest()
    {
        isHarvesting = true;
        harvestStartTime = Time.time;

        amountCollected = 0;

        anim.SetBool("isHarvesting", true);
    }

    void StopHarvest()
    {
        isHarvesting = false;
        anim.SetBool("isHarvesting", false);

        Return();
    }

    void Harvest()
    {
        if(resource == null)
        {
            StopCollecting();
            return;
        }

        if (isHarvesting)
        {
            if(Time.time - harvestStartTime > 5)
            {
                StopHarvest();
                return;
            }

            if (amountCollected == 0)
            {
                resourceParent.SetActive(true);
                icon.sprite = resource.icon.sprite;
            }

            var collected = resource.CollectResource(100);

            amountCollected += collected;

            if(collected == 0)
            {
                StopHarvest();
                return;
            }
        }
    }

    void Return()
    {
        SetMovePosition(castle.position);
    }
    
    public void StartCollecting()
    {
        resourceParent.SetActive(false);
    }

    public void StopCollecting()
    {
        isHarvesting = false;
        Return();
    }
}