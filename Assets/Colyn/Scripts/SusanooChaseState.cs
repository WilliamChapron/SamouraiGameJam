using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SusanooChaseState : State
{
    public bool playerInAttackRange;
    public bool playerInFront;
    public SusanooAttackState attackState;

    private NavMeshAgent agent;
    SusanooScript susanooScript;

    void Start()
    {
        susanooScript = GetComponentInParent<SusanooScript>();
    }

    public override State RunCurrentState()
    {
        playerInAttackRange = (transform.position - susanooScript.playerTransform.position).magnitude <= 2.5f;
        playerInFront = susanooScript.attackCollider.playerCollides;

        if (!playerInAttackRange)
        {
            susanooScript.agent.destination = susanooScript.playerTransform.position;
            return this;
        }

        susanooScript.agent.destination = transform.position;

        if (!playerInFront)
        {
            Vector3 lookDirection = susanooScript.playerTransform.position - transform.position;
            lookDirection.y = 0; // Gardez l'axe Y constant pour éviter de pencher vers le haut ou le bas
            susanooScript.transform.rotation = Quaternion.LookRotation(lookDirection);

            return this;
        }

        return attackState;
    }
}
