using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardChaseState : State
{
    public bool playerInAttackRange;
    public GuardAttackState attackState;

    GuardScript guard;

    public Transform player;

    void Start()
    {
        guard = GetComponentInParent<GuardScript>();
    }

    public override State RunCurrentState()
    {
        playerInAttackRange = (transform.position - player.position).magnitude <= 2.5f;

        if (playerInAttackRange)
        {
            return attackState;
        }

        guard.agent.destination = player.position;

        return this;
    }
}
