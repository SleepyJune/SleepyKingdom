using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class UpgradeItem : MonoBehaviour
{
    [NonSerialized]
    public UpgradeObject upgradeObject;

    public Image itemIcon;
    public Text itemName;

    public Text itemDescription;

    public void SetItem(UpgradeObject item)
    {
        upgradeObject = item;

        itemIcon.sprite = upgradeObject.image;
        itemName.text = upgradeObject.name;
    }
}