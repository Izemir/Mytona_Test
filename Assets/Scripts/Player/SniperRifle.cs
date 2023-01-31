using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SniperRifle : PlayerWeapon
{
    public override int Type => PlayerWeapon.SniperRifle;
    [SerializeField] protected Projectile BulletPrefab;
    [SerializeField] protected float Reload = 1f;
    [SerializeField] protected Transform FirePoint;
    [SerializeField] protected ParticleSystem VFX;

    protected float lastTime;

    protected override void Awake()
    {
        base.Awake();
        lastTime = Time.time - Reload;
    }

    protected virtual float GetDamage()
    {
        return GetComponent<Player>().GetDamage();
    }

    protected override async void Fire(PlayerInputMessage message)
    {
        if (Time.time - Reload < lastTime)
        {
            return;
        }

        if (!message.Fire)
        {
            return;
        }

        lastTime = Time.time;
        GetComponent<PlayerAnimator>().TriggerShoot();

        await Task.Delay(16);

        var bullet = Instantiate(BulletPrefab, FirePoint.position, transform.rotation);
        bullet.SetDamage(GetDamage());
        VFX.Play();
    }
}
