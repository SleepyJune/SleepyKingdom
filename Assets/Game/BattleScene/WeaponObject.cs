using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Game/Weapon Object")]
public class WeaponObject : GameDataObject
{
    public Sprite image;

    public int range = 8;

    public int attack = 5;

    public int defense = 0;

    public float atkSpeed = 1;

    public ProjectileObject projectile;
}
