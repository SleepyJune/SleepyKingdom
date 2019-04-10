using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class AnimalUnit : MapMobileUnit
{
    public Sprite faceImage;

    public SpriteRenderer faceRender;

    public int randomWalkRadius = 5;
    public float randomWalkFrequency = 1f;
    float lastRandomWalkTime;

    [NonSerialized]
    public AnimalSpawnTile spawnTile;

    Vector3Int spawnPosition;

    public Animal animal;

    private new void Start()
    {
        base.Start();

        spawnPosition = position;

        faceRender.sprite = faceImage;

        animal = new Animal();
        animal.animalUnitId = unitId;
    }

    private new void Update()
    {
        base.Update();

        if(Time.time - lastRandomWalkTime > randomWalkFrequency)
        {
            RandomWalk();
            lastRandomWalkTime = Time.time;
        }
    }

    void RandomWalk()
    {
        if (isMoving)
        {
            //return;
        }

        int deltaX = UnityEngine.Random.Range(0, randomWalkRadius);
        int deltaY = UnityEngine.Random.Range(0, randomWalkRadius);

        if(deltaX > 0 || deltaY > 0)
        {
            Vector3Int newPos = new Vector3Int(spawnPosition.x + deltaX, spawnPosition.y + deltaY, position.z);
            
            GameTile newTile;

            if (Pathfinder.map.TryGetValue(newPos, out newTile))
            {
                if (!newTile.isBlocked)
                {
                    SetMovePosition(newPos);
                }
            }
        }
    }

    public override void OnClickEvent()
    {
        var popup = unitManager.animalManager.capturePopupPrefab;

        if (popup != null)
        {
            var newPopup = Instantiate(popup, unitManager.popupParent);

            newPopup.SetAnimal(this);
        }
    }

    public override void Death()
    {
        unitManager.animalManager.RemoveAnimal(this);
        base.Death();
    }
}
