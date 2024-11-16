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
    
    EnemyStats enemyStats;

    void Start()
    {
        guard = GetComponentInParent<GuardScript>();
    }

    public override State RunCurrentState()
    {
        if (playerInAttackRange)
        {
            return attackState;
        }

        guard.agent.destination = player.position;

        return this;
    }
}
