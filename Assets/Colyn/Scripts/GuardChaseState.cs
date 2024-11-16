using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardChaseState : State
{
    public bool playerInAttackRange;
    public GuardAttackState attackState;

    public Transform player;
    private NavMeshAgent agent;
    EnemyStats enemyStats;

    void Start()
    {
        enemyStats = GetComponentInParent<EnemyStats>();
        
        agent = GetComponent<NavMeshAgent>();
        agent.speed = enemyStats.moveSpeed;
    }

    public override State RunCurrentState()
    {
        if (playerInAttackRange)
        {
            return attackState;
        }

        agent.destination = player.position;

        return this;
    }
}
