using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Projectile : MonoBehaviour
{
    public SpriteRenderer image;

    BattleUnit target;
    ProjectileObject projectileObject;

    Vector3 lastTargetPosition = Vector3.zero;

    bool isAlive = true;

    float speed;
    int damage;

    public void Initialize(BattleUnit target, ProjectileObject projectileObject)
    {
        this.projectileObject = projectileObject;
        this.image.sprite = projectileObject.image;
        this.target = target;

        this.speed = projectileObject.speed;
        this.damage = projectileObject.damage;
    }

    private void Update()
    {
        if (!isAlive)
        {
            return;
        }

        if (target != null)
        {
            lastTargetPosition = target.transform.position;
        }
        else if(lastTargetPosition == Vector3.zero)
        {
            //destroy this projectile
        }
        
        var dir = (lastTargetPosition - transform.position).normalized;
        var dist = speed * Time.deltaTime;
        var distLeft = Vector3.Distance(transform.position, lastTargetPosition);

        if (dir != Vector3.zero)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        if (dist >= distLeft)
        {
            dist = distLeft;
            DealDamage();
        }

        var pos = transform.position + dir * dist;

        transform.position = pos;
    }

    void DealDamage()
    {
        target.TakeDamage(damage);
        Death();
    }

    public void Death()
    {
        isAlive = false;
        Destroy(gameObject);
    }
}