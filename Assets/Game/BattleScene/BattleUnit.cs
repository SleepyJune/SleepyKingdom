using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

    public AILerp aiPath;
    public Seeker seeker;
    public Pathfinding.RVO.RVOController rvo;

    BattleUnitObject unitObj;

    BattleUnitManager unitManager;
    BattleEffectsManager effectsManager;

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

    List<BattleUnit> alliesByDistance;
    List<BattleUnit> enemiesByDistance;

    float lastUnitDistanceUpdate;

    bool isAttacking = false;
    bool isAlive = true;

    public new BoxCollider2D collider;

    Dictionary<BattleUnit, GraphUpdateObject> sharedObstacle = new Dictionary<BattleUnit, GraphUpdateObject>();

    public void Initialize(BattleUnitObject unitObj, BattleUnitTeam team, BattleUnitManager unitManager)
    {
        this.unitObj = unitObj;
        this.unitManager = unitManager;
        this.effectsManager = unitManager.effectsManager;

        icon.sprite = unitObj.spriteObj.image;

        health = unitObj.health;
        attack = unitObj.attack;
        defense = unitObj.defense;
        speed = unitObj.speed;
        range = unitObj.range;
        attackSpeed = unitObj.atkSpeed;

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

        aiPath.speed = speed / 10.0f;
        
    }

    public void PreAttack()
    {
        if(Time.time > lastUnitDistanceUpdate)
        {
            UpdateUnitDistance();
        }

        if (Time.time > attackAnimationTime)
        {
            StopAttack();
        }
    }

    private void UpdateUnitDistance()
    {
        enemiesByDistance = enemies.OrderBy(unit => Vector3.Distance(unit.transform.position, transform.position)).ToList();
        alliesByDistance = allies.OrderBy(unit => Vector3.Distance(unit.transform.position, transform.position)).ToList();

        lastUnitDistanceUpdate = Time.time + 2; //2 second update time
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

        foreach (var unit in enemiesByDistance)
        {
            if(unit.isAlive && Vector3.Distance(unit.transform.position, transform.position) <= range / 10.0f)
            {
                AttackUnit(unit);
                break;

                /*var samePlace = false;
                foreach(var otherUnits in unitManager.allUnits)
                {
                    if(otherUnits != this && otherUnits.isAttacking 
                        && Vector3.Distance(otherUnits.transform.position, otherUnits.transform.position) <= .15f)
                    {
                        samePlace = true;
                        break;
                    }
                }

                if (!samePlace)
                {
                    
                }*/
            }
        }
    }

    private void AttackUnit(BattleUnit unit)
    {
        attackAnimationTime = Time.time + attackSpeed;
        isAttacking = true;
        aiPath.canMove = false;

        //Invoke("SetPathBlockingStateEx", .5f);// (true);

        SetPathBlockingState(true);

        unit.TakeDamage(attack);

        if(unit.range <= 20)
        {
            var dir = (unit.transform.position - transform.position).normalized;
            var pos = transform.position + dir * .5f;

            effectsManager.CreateMeleeBangPrefab(pos);
        }
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

        //var dir = (targetPosition - transform.position).normalized;
        //transform.position = transform.position + dir * speed * Time.deltaTime;

        //rb.velocity = dir * speed;

        foreach (var unit in enemiesByDistance)
        {
            if (unit.isAlive && Vector3.Distance(unit.transform.position, transform.position) <= 5 + range / 10.0f)
            {
                aiPath.destination = unit.transform.position;
                return;
            }
        }

        aiPath.destination = targetPosition;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Death();
            return;
        }
    }

    public void Death()
    {
        isAlive = false;
        StopAttack();
        unitManager.removeUnits.Enqueue(this);
    }

    private void SetPathBlockingStateEx()
    {
        SetPathBlockingState(true);
    }

    private void SetPathBlockingState(bool blocking)
    {
        var bounds = collider.bounds;
        bounds.extents += new Vector3(0, 0, 10000);

        GraphUpdateObject guo = new GraphUpdateObject(bounds);

        int tag = blocking ? 1 : 0;

        //rvo.priority = blocking ? .1f : .5f;

        //guo.addPenalty = 1000 * tag;

        guo.modifyTag = true;
        guo.setTag = tag;
        
        guo.modifyWalkability = true;
        guo.setWalkability = !blocking;

        guo.updatePhysics = false;

        AstarPath.active.UpdateGraphs(guo);
                
        /*if (blocking == false)
        {
            foreach (var pair in sharedObstacle)
            {
                var obstacle = pair.Value;
                var unit = pair.Key;

                obstacle.modifyWalkability = true;
                obstacle.setWalkability = !blocking;

                AstarPath.active.UpdateGraphs(obstacle);

                if (unit.isAttacking)
                {
                    GraphUpdateObject newGuo = new GraphUpdateObject(unit.collider.bounds);

                    newGuo.modifyWalkability = true;
                    newGuo.setWalkability = !blocking;

                    newGuo.updatePhysics = false;

                    AstarPath.active.UpdateGraphs(newGuo);
                }
            }

            sharedObstacle = new Dictionary<BattleUnit, GraphUpdateObject>();
        }
        else if(isAttacking == true)
        {
            foreach(var unit in unitManager.allUnits)
            {
                if (unit != this && unit.isAttacking)
                {
                    var distance = Vector3.Distance(transform.position, unit.transform.position);
                    var radius = aiPath.radius;

                    if (distance <= radius * 4)
                    {
                        var dir = (unit.transform.position - transform.position).normalized;
                        var pos = transform.position + dir * distance * .5f;

                        Bounds merged = new Bounds(pos, new Vector3(radius, radius, 10f));
                        //merged.Encapsulate(unit.collider.bounds);
                        bounds.extents += new Vector3(0, 0, 10000);

                        Debug.Log(merged);

                        GraphUpdateObject newGuo = new GraphUpdateObject(merged);

                        newGuo.modifyTag = true;
                        newGuo.setTag = blocking ? 1 : 0;

                        newGuo.modifyWalkability = true;
                        newGuo.setWalkability = !blocking;

                        newGuo.updatePhysics = false;

                        AstarPath.active.UpdateGraphs(newGuo);

                        sharedObstacle.Add(unit, newGuo);
                    }
                }
            }
        }*/

        AstarPath.active.AddWorkItem(new AstarWorkItem(() => {
            // Safe to update graphs here
            var node = AstarPath.active.GetNearest(transform.position).node;
            node.Walkable = !blocking;
        }));

        //RecheckUnitPaths();
    }

    private void RecheckUnitPaths()
    {
        foreach(var unit in allies)
        {
            var path = unit.seeker.GetCurrentPath();
            //path.vectorPath
            if(path != null)
            {
                unit.aiPath.SearchPath();
            }
        }
    }
}