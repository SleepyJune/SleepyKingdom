using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TempleController : MonoBehaviour
{
    public Transform itemList;

    public TempleItem templeItemPrefab;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var list = GameManager.instance.gamedatabaseManager.shopItemObjects;

        foreach(var item in list.Values)
        {
            var newItem = Instantiate(templeItemPrefab, itemList);

            newItem.SetItem(item);
        }
    }
}