using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardScript : EnemyStats
{
    public NavMeshAgent agent;

    void Start()
    {
        moveSpeed = 3.0f;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

        maxAttackCooldown = 5.0f;
    }

}
