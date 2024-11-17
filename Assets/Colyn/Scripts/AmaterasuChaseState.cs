using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmaterasuChaseState : State
{
    public bool playerInAttackRange;
    public bool playerInFront;
    public AmaterasuAttackState attackState;

    AmaterasuScript amaterasu;

    void Start()
    {
        amaterasu = GetComponentInParent<AmaterasuScript>();
    }

    public override State RunCurrentState()
    {
        // Straight slow path towards player

        playerInAttackRange = (transform.position - amaterasu.playerTransform.position).magnitude <= 2.0f;

        playerInFront = amaterasu.attackCollider.playerCollides;

        if (playerInAttackRange && playerInFront)
        {
            return attackState;
        }

        amaterasu.agent.destination = amaterasu.playerTransform.position;

        return this;
    }
}
