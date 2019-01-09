using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MapInteractableManager : MonoBehaviour
{
    [NonSerialized]
    public Dictionary<Vector3, MapInteractableUnit> interactables = new Dictionary<Vector3, MapInteractableUnit>();

    MapUnitManager unitManager;

    private void Start()
    {
        unitManager = GetComponent<MapUnitManager>();

        Initialize();
    }

    private void Initialize()
    {
        foreach (var save in unitManager.mapDatabase.interactableSpawnTileDictionary.Values)
        {
                        
            if (!interactables.ContainsKey(save.position))
            {
                Load(save);
            }
        }
    }

    void Load(InteractableSpawnTile save)
    {        
        if (save.prefab != null)
        {
            var newInteractable = Instantiate(save.prefab, unitManager.unitParent);
            newInteractable.position = save.position;

            unitManager.InitializeUnit(newInteractable);

            interactables.Add(save.position, newInteractable);
        }
    }
}
