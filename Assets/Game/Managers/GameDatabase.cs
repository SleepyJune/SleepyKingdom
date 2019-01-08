using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "Game/GameDatabase")]

public class GameDatabase : ScriptableObject
{
    public GameDataObject[] allObjects = new GameDataObject[0];

    public MapUnit[] allPrefabs = new MapUnit[0];
}