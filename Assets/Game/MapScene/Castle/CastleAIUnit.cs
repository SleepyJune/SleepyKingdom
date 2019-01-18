using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CastleAIUnit : MonoBehaviour
{
    public float updateInterval = 2.0f;

    float lastUpdateTime;

    MapCastleUnit unit;
    MapUnitManager unitManager;

    private void Start()
    {
        unit = GetComponent<MapCastleUnit>();
        unitManager = unit.unitManager;
    }

    private void Update()
    {
        if(Time.time - lastUpdateTime > updateInterval)
        {
            UpdateAI();
            lastUpdateTime = Time.time;
        }
    }

    void UpdateAI()
    {
        if(unit.Distance(unitManager.myCastle) <= unit.country.detectionRadius)
        {
            unit.SetMovePosition(unitManager.myCastle.position);
        }
    }
}
