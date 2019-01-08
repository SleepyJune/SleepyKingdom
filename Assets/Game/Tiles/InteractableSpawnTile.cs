using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "Game/Tiles/Interactable Spawn Tile")]
public class InteractableSpawnTile : SpawnTile
{
    public MapInteractableUnit prefab;
}