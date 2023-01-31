using System;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{

    private void Awake()
    {
        
        EventBus<PlayerInputMessage>.Sub(PlayerMoveSubscriber);
    }
    private void OnDestroy()
    {
        EventBus<PlayerInputMessage>.Unsub(PlayerMoveSubscriber);        
    }

    private void PlayerMoveSubscriber(PlayerInputMessage message)
	{
		if(message.MovementDirection.x!=0 || message.MovementDirection.y != 0)
        {
            var speed = GetComponent<Player>().GetMoveSpeed();
            var delta = new Vector3(speed * message.MovementDirection.x, 0, speed * message.MovementDirection.y) * Time.deltaTime;
            transform.position += delta;
        }
            transform.forward = new Vector3(message.AimDirection.x, 0, message.AimDirection.y);


    }
}