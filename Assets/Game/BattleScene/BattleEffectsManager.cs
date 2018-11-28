using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BattleEffectsManager : MonoBehaviour
{
    public Transform effectsParent;

    public GameObject meleeBangPrefab;

    public void CreateMeleeBangPrefab(Vector3 pos)
    {
        var newObj = Instantiate(meleeBangPrefab, effectsParent);
        newObj.transform.position = pos;
    }
}