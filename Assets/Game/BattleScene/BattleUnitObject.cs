using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "Game/Unit Object")]
public class BattleUnitObject : GameDataObject
{
    public Sprite image;

    public int health = 10;

    public int attack = 10;
    public int defense = 10;

    public int speed = 10;

    public float atkSpeed = 1;

    public int range = 1;

    public int populationCost = 10;

    public ProjectileObject projectileObject;
}