using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public Projectile projectilePrefab;

    public Transform unitParent;

    BattleUnitManager unitManager;

    private void Start()
    {
        unitManager = GetComponent<BattleUnitManager>();
    }

    public void CreateProjectile(BattleUnit owner, BattleUnit target, ProjectileObject projectileObject)
    {
        var newProjectile = Instantiate(projectilePrefab, unitParent);
        newProjectile.transform.position = owner.transform.position;

        var start = owner.transform.position;
        var end = target.transform.position;

        List<BattleUnit> targets;

        if(owner.team == BattleUnitTeam.Player)
        {
            targets = unitManager.computerUnits;
        }
        else
        {
            targets = unitManager.playerUnits;
        }

        newProjectile.Initialize(owner, start, end, projectileObject, targets);
    }
}