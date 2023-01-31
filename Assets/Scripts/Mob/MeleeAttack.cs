using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(MobMover))]
[RequireComponent(typeof(Mob))]
public class MeleeAttack : MonoBehaviour, IMobComponent
{
    [SerializeField] protected float AttackDistance = 1f;
    [SerializeField] protected float DamageDistance = 1f;

    [SerializeField] protected float AttackDelay = 1f;

    [SerializeField] private bool IsAttackKamikaze = false;
    
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
		if (playerDistance <= DamageDistance)
		{
			Player.Instance.TakeDamage(mob.GetDamage());
			if(IsAttackKamikaze) {
				mob.TakeDamage(1); 
			}
		}
		mover.Active = true;
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

    public void AddListenerOnAttack(Action attack)
    {
		OnAttack += attack;
    }
}