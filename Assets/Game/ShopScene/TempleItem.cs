using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class TempleItem : MonoBehaviour
{
    [NonSerialized]
    public ShopItemObject shopItem;

    public Image itemIcon;
    public Text itemName;

    public void SetItem(ShopItemObject item)
    {
        shopItem = item;

        itemIcon.sprite = shopItem.image;
        itemName.text = shopItem.name;
    }
}