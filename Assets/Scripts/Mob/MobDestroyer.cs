using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobDestroyer : MonoBehaviour, IMobComponent
{
    [SerializeField] private int destroyDelay = 2;
    public void OnDeath()
    {
        Destroy(gameObject, destroyDelay);
    }

}
