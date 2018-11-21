using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public enum BattleUnitTeam
{
    Player,
    Computer,
}

public class BattleUnit : MonoBehaviour
{
    public Image icon;

    public Image hpBar;

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

    Rigidbody2D rb;

    public void Initialize(BattleUnitObject unitObj, BattleUnitTeam team, BattleUnitManager unitManager)
    {
        rb = GetComponent<Rigidbody2D>();

        this.unitObj = unitObj;

        icon.sprite = unitObj.spriteObj.image;

        health = unitObj.health;
        attack = unitObj.attack;
        defense = unitObj.defense;
        speed = unitObj.speed;
        range = unitObj.range;

        this.team = team;
        
        if(team == BattleUnitTeam.Player)
        {
            allies = unitManager.playerUnits;
            enemies = unitManager.computerUnits;
            targetPosition = unitManager.redCastlePosition.position;
            hpBar.color = Color.red;
        }
        else
        {
            allies = unitManager.computerUnits;
            enemies = unitManager.playerUnits;
            targetPosition = unitManager.blueCastlePosition.position;
            hpBar.color = Color.blue;
        }        
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
            if(Vector3.Distance(unit.transform.position, transform.position) <= range)
            {
                attackAnimationTime = Time.time + attackSpeed;
            }
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

        var dir = (targetPosition - transform.position).normalized;
        //transform.position = transform.position + dir * speed * Time.deltaTime;

        rb.velocity = dir * speed;
    }

    public void Death()
    {
        unitManager.removeUnits.Enqueue(this);
    }
}