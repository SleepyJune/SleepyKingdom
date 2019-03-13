using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class MyCountry
{
    public string name;

    public float population;
    public float maxCapacity;

    public int gold;
    public int gems;

    public MyCountry(string newName)
    {
        name = newName;
    }

}
