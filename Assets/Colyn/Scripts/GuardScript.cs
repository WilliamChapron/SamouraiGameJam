using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardScript : MonoBehaviour
{
    public NavMeshAgent agent;
    EnemyStats enemyStats;


    void Start()
    {
        enemyStats = GetComponentInParent<EnemyStats>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = enemyStats.moveSpeed;
    }

}
