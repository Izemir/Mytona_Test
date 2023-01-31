using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperProjectile : Projectile
{
    public override void SetDamage(float damage)
    {
        Damage = damage*2;
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (destroyed)
        {
            return;
        }
        if (DamagePlayer && other.CompareTag("Player"))
        {
            other.GetComponent<Player>().TakeDamage(Damage);
        }

        if (DamageMob && other.CompareTag("Mob"))
        {
            var mob = other.GetComponent<Mob>();
            mob.TakeDamage(Damage);
        }
    }
}
