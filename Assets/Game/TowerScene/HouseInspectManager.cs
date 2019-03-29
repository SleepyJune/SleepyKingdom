using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class HouseInspectManager : MonoBehaviour
{
    public Popup housePopup;

    House currentSelectedHouse;

    public void SetHouse(House house)
    {
        housePopup.Show();
    }
}
