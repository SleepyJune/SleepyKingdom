using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class Ship
{
    public string mapName;

    public string lastMapName = "PEI";
    public Vector3Int lastMapPosition = new Vector3Int(0,0,0);

    public Vector3Int position;
    public Quaternion rotation;

    public int water;
    public int wheat;
    public int stone;
    public int wood;

    float resourceCollectRate;
}