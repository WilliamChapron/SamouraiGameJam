using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAttackState : State
{
    GuardScript enemyStats;
    public GuardIdleState idleState;

    void Start()
    {
        enemyStats = GetComponentInParent<GuardScript>();
    }

    public override State RunCurrentState()
    {
        // Attack
        Debug.Log("Attacks");
        enemyStats.Attack();

        return idleState;
    }
}
