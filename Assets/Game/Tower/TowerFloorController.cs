using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TowerFloorController : MonoBehaviour
{
    [NonSerialized]
    public TowerFloor floor;

    public TowerSlotController[] slots;

    private void Start()
    {
        if (GameManager.instance.sceneChanger)
        {
            floor = GameManager.instance.sceneChanger.targetFloor;
        }

        if (floor == null)
        {
            //floor = TowerFloor.Generate();
        }

        Initialize();
    }

    public void Initialize()
    {
        if(floor != null)
        {
            for(int i=0;i< floor.buildings.Length;i++)
            {
                var building = floor.buildings[i];

                if(building.buildingID != 0)
                {
                    slots[i].SetBuilding(building);
                }
            }
        }
    }
}