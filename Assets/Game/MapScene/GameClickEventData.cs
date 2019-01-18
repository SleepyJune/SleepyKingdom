using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GameClickEventData
{
    public TouchInput input;

    public bool isUsed = false;

    public GameClickEventData(TouchInput input)
    {
        this.input = input;
    }
}