using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardIdleState : State
{
    public GuardFormation formation;
    public bool playerInSight;
    public GuardChaseState chaseState;
    GuardScript guard;

    Vector3 formationPosition;

    bool isAttacking = false;

    void Start()
    {
        formationPosition = transform.position;
        guard = GetComponentInParent<GuardScript>();
    }

    public override State RunCurrentState()
    {
        // Go to formation point.

        guard.agent.destination = formationPosition;

        return this;
    }

    public void SetFormationPosition(Vector3 position)
    {
        formationPosition = position;
    }

}
