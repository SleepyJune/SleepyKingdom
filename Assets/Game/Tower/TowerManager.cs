using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public Tower tower;

    public TowerFloorItem floorPrefab;

    public Transform floorList;

    private void Start()
    {
        tower = Tower.Generate();

        SetTower(tower);
    }

    public void SetTower(Tower tower)
    {
        this.tower = tower;

        for(int i=tower.floors.Length-1;i>=0;i--)
        {
            var floor = tower.floors[i];

            if(floor == null)
            {
                continue;
            }

            var newFloor = Instantiate(floorPrefab, floorList);
            newFloor.SetFloor(floor);
        }
    }

}