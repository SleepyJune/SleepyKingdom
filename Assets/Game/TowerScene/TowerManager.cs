using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [NonSerialized]
    public HouseSlotManager houseSlotManager;

    [NonSerialized]
    public HousePopupManager housePopupManager;

    public HouseInspectPopup houseInspectPopup;

    public static TowerManager instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        houseSlotManager = GetComponent<HouseSlotManager>();
        housePopupManager = GetComponent<HousePopupManager>();
    }
}
