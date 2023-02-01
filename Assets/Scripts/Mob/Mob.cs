using System;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
    [SerializeField] protected int MobId=0;
    [SerializeField] protected float Damage = 1;
    [SerializeField] protected float MoveSpeed = 3.5f;
    [SerializeField] protected float Health = 3;
    [SerializeField] protected float MaxHealth = 3;
    
    private Action<float, float> OnHPChange = null;

    public int GetId()
    {
        return MobId;
    }

    public float GetDamage()
    {
        return Damage;
    }

    public float GetMaxHealth() { return MaxHealth; }

    public void AddListenerOnHPChange(Action<float, float> action)
    {
        OnHPChange += action;
    }

    public void TakeDamage(float amount)
    {
        if (Health <= 0)
            return;
        Health -= amount;
        OnHPChange?.Invoke(Health,-amount);
        if (Health <= 0)
        {
            Death();
        }
    }
    
    public void Heal(float amount)
    {
        if (Health <= 0)
            return;
        Health += amount;
        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }
        OnHPChange?.Invoke(Health,amount);
    }

    public void Death()
    {
        EventBus.Pub(EventBus.MOB_KILLED);
        var components = GetComponents<IMobComponent>();
        foreach (var component in components)
        {
            component.OnDeath();
        }
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }
}