using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class InteractablePopup : Popup
{
    public Image icon;

    [NonSerialized]
    public MapInteractableUnit unit;
}
