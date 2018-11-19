using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class UpgradeObject : SpriteObject
{
    public string upgradeDescription;

    public float baseStatValue = 0;

    public float valueIncreament = .1f;

    public int baseCost = 100;
    public int costIncreament = 50;
    public float costIncreamentExponential = 2f;

    public int maxLevel = 10;

    public int defaultMockLevel = 5;
}