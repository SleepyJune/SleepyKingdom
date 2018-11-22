using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Pathfinding;

public enum BattleUnitTeam
{
    Player,
    Computer,
}

public class BattleUnit : MonoBehaviour
{
    public SpriteRenderer icon;

    public SpriteRenderer teamColor;

    public AIPath aiPath;

    BattleUnitObject unitObj;

    BattleUnitManager unitManager;

    int health;

    int attack;
    int defense;

    int speed;

    int range;

    BattleUnit attackTarget;

    BattleUnitTeam team;

    Vector3 targetPosition;

    float attackAnimationTime;
    float attackSpeed;

    List<BattleUnit> allies;
    List<BattleUnit> enemies;

    bool isAttacking = false;

    public new CircleCollider2D collider;

    public void Initialize(BattleUnitObject unitObj, BattleUnitTeam team, BattleUnitManager unitManager)
    {
        this.unitObj = unitObj;

        icon.sprite = unitObj.spriteObj.image;

        health = unitObj.health;
        attack = unitObj.attack;
        defense = unitObj.defense;
        speed = unitObj.speed;
        range = unitObj.range;
        attackSpeed = unitObj.atkSpeed * 1000;

        this.team = team;
        
        if(team == BattleUnitTeam.Player)
        {
            allies = unitManager.playerUnits;
            enemies = unitManager.computerUnits;
            targetPosition = unitManager.enemyCastlePosition.position;
            teamColor.color = Color.red;
        }
        else
        {
            allies = unitManager.computerUnits;
            enemies = unitManager.playerUnits;
            targetPosition = unitManager.playerCastlePosition.position;
            teamColor.color = Color.blue;
        }

        aiPath.maxSpeed = speed / 10.0f;
    }

    public void Attack()
    {
        if(health <= 0)
        {
            Death();
            return;
        }

        if (Time.time <= attackAnimationTime)
        {
            return;
        }

        foreach (var unit in enemies)
        {
            if(Vector3.Distance(unit.transform.position, transform.position) <= range / 10.0f)
            {
                AttackUnit(unit);
                break;
            }
        }
    }

    private void AttackUnit(BattleUnit unit)
    {
        attackAnimationTime = Time.time + attackSpeed;
        isAttacking = true;
        aiPath.canMove = false;
        SetPathBlockingState(true);
    }

    private void StopAttack()
    {
        if (isAttacking)
        {
            isAttacking = false;
            aiPath.canMove = true;
            SetPathBlockingState(false);
        }
    }

    public void Move()
    {
        if (health <= 0)
        {
            Death();
            return;
        }

        if(Time.time <= attackAnimationTime)
        {
            return;
        }
        else
        {
            StopAttack();
        }

        //var dir = (targetPosition - transform.position).normalized;
        //transform.position = transform.position + dir * speed * Time.deltaTime;

        //rb.velocity = dir * speed;

        aiPath.destination = targetPosition;
    }

    public void Death()
    {
        StopAttack();
        unitManager.removeUnits.Enqueue(this);
    }

    private void SetPathBlockingState(bool blocking)
    {
        var bounds = collider.bounds;
        bounds.size += new Vector3(0, 0, 5);

        GraphUpdateObject guo = new GraphUpdateObject(bounds);

        int tag = blocking ? 1 : 0;

        guo.modifyTag = true;
        guo.setTag = tag;
        guo.updatePhysics = false;

        AstarPath.active.UpdateGraphs(guo);
    }
}