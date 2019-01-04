using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "Game/Unit Object")]
public class BattleUnitObject : GameDataObject
{
    public Sprite image;

    public int health = 10;

    public int speed = 10;    

    public int populationCost = 10;

    public float populationMultiplier = .5f;
        
    public WeaponObject weaponObject;

    public HelmetObject helmetObject;

    public ProjectileObject projectileObject;
}