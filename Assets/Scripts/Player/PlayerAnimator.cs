using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator Animator;
	
	private void Awake()
	{
        EventBus<PlayerInputMessage>.Sub(PlayerAnimationSubscriber);
        EventBus.Sub(AnimateDeath,EventBus.PLAYER_DEATH);
	}

	private void OnDestroy()
	{
		EventBus.Unsub(AnimateDeath,EventBus.PLAYER_DEATH);
        EventBus<PlayerInputMessage>.Unsub(PlayerAnimationSubscriber);
    }

    private void PlayerAnimationSubscriber(PlayerInputMessage message)
    {
        Animator.SetBool("IsRun", message.MovementDirection.sqrMagnitude > 0);
    }

    private void AnimateDeath()
	{
		Animator.SetTrigger("Death");
	}

	public void TriggerShoot()
	{
		Animator.SetTrigger("Shoot");
	}
}