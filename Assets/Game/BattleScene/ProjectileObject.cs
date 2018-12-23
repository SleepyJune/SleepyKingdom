using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "Game/Projectile Object")]
public class ProjectileObject : GameDataObject
{
    public Sprite image;

    public int damage = 5;
    public int speed = 1;

    public int range = 20;
}