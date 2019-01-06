using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "Game/Map Database")]

public class MapDatabase : ScriptableObject
{
    public MapDataObject[] allObjects = new MapDataObject[0];
}