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

    public SpriteRenderer weapon;

    public SpriteRenderer teamColor;

    public AILerp aiPath;
    public Seeker seeker;
    //public Pathfinding.RVO.RVOController rvo;

    public float radius = .25f;

    [NonSerialized]
    public GraphNode blockedNode;

    BattleUnitObject unitObj;

    BattleUnitManager unitManager;
    BattleEffectsManager effectsManager;
    ProjectileManager projectileManager;

    Animator anim;

    Vector3 targetPosition;

    int health;

    int attack;
    int defense;

    [NonSerialized]
    public int speed;

    [NonSerialized]
    public int range;

    BattleUnit attackTarget;

    BattleUnitTeam team;

    Vector3 targetGoal;

    float attackAnimationTime;
    float attackSpeed;

    List<BattleUnit> allies;
    List<BattleUnit> enemies;

    List<BattleUnit> alliesByDistance;
    List<BattleUnit> enemiesByDistance;

    float lastUnitDistanceUpdate;

    bool isAttacking = false;
    bool isAlive = true;

    public new CircleCollider2D collider;

    Dictionary<BattleUnit, GraphUpdateObject> sharedObstacle = new Dictionary<BattleUnit, GraphUpdateObject>();

    Transform handTransform;

    public void Initialize(BattleUnitObject unitObj, BattleUnitTeam team, BattleUnitManager unitManager)
    {
        this.unitObj = unitObj;
        this.unitManager = unitManager;
        this.effectsManager = unitManager.effectsManager;
        this.projectileManager = unitManager.projectileManager;

        icon.sprite = unitObj.image;

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
            targetGoal = unitManager.enemyCastlePosition.position;
            teamColor.color = Color.red;
        }
        else
        {
            allies = unitManager.computerUnits;
            enemies = unitManager.playerUnits;
            targetGoal = unitManager.playerCastlePosition.position;
            teamColor.color = Color.blue;

            transform.Rotate(0, -180, 0, Space.Self);
        }

        aiPath.speed = speed / 10.0f;

        anim = GetComponent<Animator>();

        handTransform = weapon.transform.Find("Hand");

        weapon.sprite = unitObj.weaponObject.image;

        collider = transform.Find("Collider").GetComponent<CircleCollider2D>();
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

        /*var node = AstarPath.active.GetNearest(transform.position).node;
        foreach (var unit in alliesByDistance)
        {
            if(unit.blockedNode == node)
            {
                return;
            }
        }*/

        foreach (var unit in enemiesByDistance)
        {
            if(unit.isAlive && Vector3.Distance(unit.transform.position, transform.position) <= unitObj.weaponObject.range / 10.0f)
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

        //Invoke("SetPathBlockingStateEx", .5f);// (true);

        SetPathBlockingState(true);
                
        if(unitObj.weaponObject.range <= 10)
        {
            var dir = (unit.transform.position - transform.position).normalized;
            var pos = transform.position + dir * radius;

            effectsManager.CreateMeleeBangPrefab(pos);
            unit.TakeDamage(attack);
        }
        else
        {
            projectileManager.CreateProjectile(this, unit, unitObj.projectileObject);
        }

        var direction = (unit.transform.position - handTransform.position);

        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            weapon.transform.parent.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        anim.SetBool("isAttacking", true);
    }

    private void StopAttack()
    {
        if (isAttacking)
        {
            isAttacking = false;
            aiPath.canMove = true;
            SetPathBlockingState(false);

            weapon.transform.parent.rotation = Quaternion.identity;

            anim.SetBool("isAttacking", false);
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

        foreach (var unit in enemiesByDistance)
        {
            if (unit.isAlive && Vector3.Distance(unit.transform.position, transform.position) <= 5 + range / 10.0f)
            {
                aiPath.destination = unit.transform.position;
                targetPosition = unit.transform.position;
                return;
            }
        }

        targetPosition = targetGoal;
        aiPath.destination = targetGoal;
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
        
        //guo.modifyWalkability = true;
        //guo.setWalkability = !blocking;

        guo.updatePhysics = false;

        AstarPath.active.UpdateGraphs(guo);

        /*var node = AstarPath.active.GetNearest(transform.position).node;
        if (blocking)
        {
            blockedNode = node;
        }
        else
        {
            blockedNode = null;
        }*/
                
        /*AstarPath.active.AddWorkItem(new AstarWorkItem(() => {
            // Safe to update graphs here
            var node = AstarPath.active.GetNearest(transform.position).node;
            node.Walkable = !blocking;
        }));*/

        RecheckUnitPaths();
    }

    private void RecheckUnitPaths()
    {
        foreach(var unit in allies)
        {
            var path = unit.seeker.GetCurrentPath();
            
            if(path != null && path.IsDone())
            {
                foreach(var node in path.vectorPath)
                {
                    var dist = Vector2.Distance(node, transform.position);

                    if(dist <= radius * 2)
                    {
                        unit.aiPath.SearchPath();
                    }
                }                
            }
        }
    }
}