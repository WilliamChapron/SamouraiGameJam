using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAttackState : State
{
    GuardScript enemyStats;
    public GuardIdleState idleState;

    float attackTime = 0.5f;
    float maxAttackTime = 0.5f;

    void Start()
    {
        enemyStats = GetComponentInParent<GuardScript>();
    }

    public override State RunCurrentState()
    {
        attackTime -= Time.deltaTime;

        if (attackTime < 0)
        {
            attackTime = maxAttackTime;
            return idleState;
        }

        return this;
    }
}
