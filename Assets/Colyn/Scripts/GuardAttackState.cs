using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAttackState : State
{
    EnemyStats enemyStats;
    GuardIdleState idleState;

    void Start()
    {
        enemyStats = GetComponentInParent<EnemyStats>();
    }

    public override State RunCurrentState()
    {
        // Attack
        Debug.Log("Attacks");
        enemyStats.Attack();

        return idleState;
    }
}
