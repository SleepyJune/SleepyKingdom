using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class UnitClickHandler : MonoBehaviour
{
    public Unit unit;

    private void OnMouseDown()
    {
        unit.OnMouseDownEvent();
    }
}