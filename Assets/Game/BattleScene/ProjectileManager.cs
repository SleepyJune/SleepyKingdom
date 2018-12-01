using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public Projectile projectilePrefab;

    public Transform unitParent;

    public void CreateProjectile(BattleUnit owner, BattleUnit target, ProjectileObject projectileObject)
    {
        var newProjectile = Instantiate(projectilePrefab, unitParent);
        newProjectile.transform.position = owner.transform.position;
        newProjectile.Initialize(target, projectileObject);
    }
}