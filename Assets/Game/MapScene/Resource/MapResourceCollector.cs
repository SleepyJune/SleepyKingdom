using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MapResourceCollector : MapUnit
{
    MapResource mapResource;

    MapCastleUnit castle;

    public void SetReousrce(MapResource resource)
    {
        castle = unitManager.myCastle;
        mapResource = resource;

        position = castle.position;
        SetMovePosition(resource.position);

        StartCollecting();
    }

    public void StartCollecting()
    {

    }

    public void StopCollecting()
    {
        
    }

    protected override void Start()
    {
        base.Start();


    }
}