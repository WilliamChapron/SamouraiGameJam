using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SuzanooChaseState : State
{
    public bool playerInAttackRange;
    public SuzanooAttackState attackState;

    public Transform player;
    private NavMeshAgent agent;
    EnemyScript enemyStats;

    void Start()
    {
        enemyStats = GetComponentInParent<EnemyScript>();
        
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
