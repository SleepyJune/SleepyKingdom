using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class UnitClickHandler : MonoBehaviour
{
    public MapUnit unit;

    private void OnMouseDown()
    {
        unit.OnClickEvent();
    }
}