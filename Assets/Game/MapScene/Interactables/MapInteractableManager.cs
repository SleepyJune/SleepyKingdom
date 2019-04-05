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

    public void Initialize()
    {
        Unload();

        interactables = new Dictionary<Vector3, MapInteractableUnit>();
        
        foreach (var save in GameManager.instance.gamedatabaseManager.currentMap.interactableSpawnTileDictionary.Values)
        {                        
            if (!interactables.ContainsKey(save.position))
            {
                Load(save);
            }
        }
    }

    public MapPortalUnit FindPortal(string mapName)
    {
        foreach(var interactable in interactables.Values)
        {
            if(interactable is MapPortalUnit)
            {
                var portal = interactable as MapPortalUnit;
                if(portal.mapName == mapName)
                {
                    return portal;
                }
            }
        }

        return null;
    }

    void Load(InteractableSpawnTile save)
    {        
        if (save.prefab != null)
        {
            var newInteractable = Instantiate(save.prefab, unitManager.unitParent);
            newInteractable.position = save.position;

            interactables.Add(save.position, newInteractable);

            unitManager.AddUnit(newInteractable);
        }
    }

    public void Unload()
    {
        foreach(var interactable in interactables.Values)
        {
            Destroy(interactable.gameObject);
        }
    }
}
