using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Projectile : MonoBehaviour
{
    public SpriteRenderer image;

    BattleUnit target;
    ProjectileObject projectileObject;

    //Vector3 lastTargetPosition = Vector3.zero;

    Vector3 start;
    Vector3 end;

    Vector3 direction;

    bool isAlive = true;

    float speed;
    int damage;

    LayerMask layerMask;

    BattleUnit owner;

    List<BattleUnit> targets;

    public void Initialize(BattleUnit owner, Vector3 start, Vector3 end, ProjectileObject projectileObject, List<BattleUnit> targets)
    {
        this.projectileObject = projectileObject;
        this.image.sprite = projectileObject.image;

        this.speed = projectileObject.speed;
        this.damage = projectileObject.damage;

        this.start = start;
        this.end = end;

        direction = (end - start).normalized;

        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        layerMask = LayerMask.GetMask("Unit");

        this.owner = owner;
        this.targets = targets;
    }

    private void Update()
    {
        if (!isAlive)
        {
            return;
        }   
        
        if(Vector3.Distance(start, transform.position) > projectileObject.range / 10.0f)
        {
            Death();
            return;
        }
        
        var dist = speed * Time.deltaTime;

        foreach(var unit in targets)
        {
            var point = unit.transform.position;

            var projectedPoint = Vector3.Project((point - start), (end - start)) + start;
            if(Vector3.Distance(projectedPoint, transform.position) <= dist)
            {
                DealDamage(unit);
            }
        }

        /*var collisions = Physics2D.RaycastAll(transform.position, direction, dist, layerMask);

        foreach(var collision in collisions)
        {
            var unit = collision.transform.parent.GetComponent<BattleUnit>();
            if(unit != null &&)
        }

        var distLeft = Vector3.Distance(transform.position, lastTargetPosition);
               
        if (dist >= distLeft)
        {
            dist = distLeft;
            DealDamage();
        }*/

        var pos = transform.position + direction * dist;

        transform.position = pos;
    }

    void DealDamage(BattleUnit unit)
    {
        unit.TakeDamage(damage);
        Death();
    }

    public void Death()
    {
        isAlive = false;
        Destroy(gameObject);
    }
}