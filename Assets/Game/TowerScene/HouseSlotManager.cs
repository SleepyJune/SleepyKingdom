using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class HouseSlotManager : MonoBehaviour
{
    List<HouseSlot> cageSlots = new List<HouseSlot>();

    private void Start()
    {
        
    }

    public void AddCageSlot(HouseSlot slot)
    {
        cageSlots.Add(slot);
    }
}
