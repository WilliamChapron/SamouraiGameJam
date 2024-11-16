using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardIdleState : State
{
    EnemyStats enemyStats;
    public GuardFormation formation;
    public bool playerInSight;
    public GuardChaseState chaseState;

    Vector3 formationPosition;

    private NavMeshAgent agent;

    bool isAttacking = false;

    void Start()
    {
        enemyStats = GetComponentInParent<EnemyStats>();
        formationPosition = transform.position;
        //formation = enemyStats.GetComponentsInParent<GuardFormation>();
    }

    public override State RunCurrentState()
    {
        // Go to formation point.
        
        agent.destination = formationPosition;

        return this;
    }

    public void SetFormationPosition(Vector3 position)
    {
        formationPosition = position;
    }

}
