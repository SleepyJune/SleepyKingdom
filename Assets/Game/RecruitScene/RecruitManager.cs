using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class RecruitManager : MonoBehaviour
{
    public Transform listTransform;

    public RecruitItem itemPrefab;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var list = GameManager.instance.gamedatabaseManager.battleUnitObjects;

        foreach (var item in list.Values)
        {
            var newItem = Instantiate(itemPrefab, listTransform);

            newItem.SetItem(item);
        }
    }
}