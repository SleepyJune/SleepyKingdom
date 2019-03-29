using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class HouseSlotManager : MonoBehaviour
{
    List<HouseSlot> houseSlots = new List<HouseSlot>();

    private void Start()
    {
        
    }

    public void AddCageSlot(HouseSlot slot)
    {
        houseSlots.Add(slot);
    }
}
