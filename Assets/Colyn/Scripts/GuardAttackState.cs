using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAttackState : State
{
    GuardScript guard;
    public GuardIdleState idleState;

    float attackTime = 0.5f;
    float maxAttackTime = 0.5f;

    void Start()
    {
        guard = GetComponentInParent<GuardScript>();
    }

    public override State RunCurrentState()
    {

        guard.agent.destination = guard.transform.position;

        attackTime -= Time.deltaTime;

        if (attackTime < 0)
        {
            attackTime = maxAttackTime;
            return idleState;
        }

        return this;
    }
}
