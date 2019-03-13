using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MapShipManager : MonoBehaviour
{
    public MapShip shipPrefab;

    [NonSerialized]
    public MapShip myShip;

    MapUnitManager unitManager;

    private void Start()
    {
        unitManager = GetComponent<MapUnitManager>();

        myShip = Instantiate(shipPrefab, unitManager.unitParent);

        unitManager.myShip = myShip;
    }
}
