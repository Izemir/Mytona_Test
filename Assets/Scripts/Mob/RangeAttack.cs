using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MobMover))]
[RequireComponent(typeof(Mob))]
public class RangeAttack : MonoBehaviour,IMobComponent
{
    [SerializeField] protected float AttackDistance = 5f;
    [SerializeField] protected float AttackDelay = .5f;
	[SerializeField] protected float AttackCooldown = 2f;
    [SerializeField] protected Projectile Bullet;
    
	private MobMover mover;
	private Mob mob;
	private MobAnimator mobAnimator;
	private bool attacking = false;
	private Coroutine _attackCoroutine = null;
    private Action OnAttack;

    private void Awake()
	{
		mob = GetComponent<Mob>();
		mover = GetComponent<MobMover>();
		mobAnimator = GetComponent<MobAnimator>();
		EventBus.Sub(OnDeath,EventBus.PLAYER_DEATH);
	}

	private void OnDestroy()
	{
		EventBus.Unsub(OnDeath,EventBus.PLAYER_DEATH);
	}

	private void Update()
	{
		if (attacking)
		{
			return;
		}
		var playerDistance = (transform.position - Player.Instance.transform.position).Flat().magnitude;
		if (playerDistance <= AttackDistance)
		{
			attacking = true;
			_attackCoroutine = StartCoroutine(Attack());
		}
	}

	private IEnumerator Attack()
	{
		OnAttack?.Invoke();
		mobAnimator.StartAttackAnimation();
		mover.Active = false;
		yield return  new WaitForSeconds(AttackDelay);
		var playerDistance = (transform.position - Player.Instance.transform.position).Flat().magnitude;
		if (playerDistance <= AttackDistance)
		{
			var bullet = Instantiate(Bullet, transform.position,
				Quaternion.LookRotation((Player.Instance.transform.position - transform.position).Flat().normalized,
					Vector3.up));
			bullet.SetDamage(mob.GetDamage());
		}
		mover.Active = true;
		yield return  new WaitForSeconds(AttackCooldown);
		attacking = false;
		_attackCoroutine = null;
	}
	
	public void OnDeath()
	{
		enabled = false;
		if (_attackCoroutine != null)
		{
			StopCoroutine(_attackCoroutine);
		}
	}

    internal void AddListenerOnAttack(Action attack)
    {
        OnAttack += attack;
    }
}