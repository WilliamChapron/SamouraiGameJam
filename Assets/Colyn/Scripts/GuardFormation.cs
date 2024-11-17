using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardFormation : MonoBehaviour
{
    public bool isSomebodyAttacking;

    public GameObject guardPrefab;

    GameObject[] guards = new GameObject[3];

    StateManager[] guardStateManagers = new StateManager[3];

    int nextGuardToAttack = 0;

    bool[] isGuardAttacking = new bool[3];

    GuardScript[] guardScripts = new GuardScript[3];

    HealthComponent[] healthComponents = new HealthComponent[3];

    GameObject playerObject;

    float maxTimeUntilNextAttack = 2.0f;

    float timeUntilNextAttack = 2.0f;

    float maxTimeUntilNextOffset = 1.5f;

    float timeUntilNextOffset = 1.5f;

    float maxRandomTimeUntilNextOffset = 1f;

    Vector3[] randomGuardOffset = new Vector3[3];

    float randomOffsetRange = 0.8f;

    bool[] deadEnemyIndex = new bool[3];

    Vector3[] enemyOffset = new Vector3[3];

    void Start()
    {
        enemyOffset[0] = new Vector3(-4, 0, 0);
        enemyOffset[1] = new Vector3(2, 0, -5);
        enemyOffset[2] = new Vector3(2, 0, 5);

        for (int i = 0; i < 3; i++)
        {
            deadEnemyIndex[i] = false;
            guards[i] = Instantiate(guardPrefab, transform.position + enemyOffset[i], transform.rotation, transform);
            guardStateManagers[i] = guards[i].GetComponent<StateManager>();
            guardScripts[i] = guards[i].GetComponent<GuardScript>();
            healthComponents[i] = guards[i].GetComponent<HealthComponent>();
        }    

        playerObject = GameObject.Find("Player");
    }

    private void Update()
    {
        Vector3 playerPosition = playerObject.transform.position;

        for (int i = 0; i < 3; i++)
        {
            if( deadEnemyIndex[i])
                { continue; }

            if (!guards[i])
            {
                deadEnemyIndex[i] = true;
                isGuardAttacking[i] = false;
                continue;
            }

            isGuardAttacking[i] = guardStateManagers[i].currentState.name == "ChaseState";
        }

        isSomebodyAttacking = isGuardAttacking[0] || isGuardAttacking[1] || isGuardAttacking[2];

        timeUntilNextOffset -= Time.deltaTime;

        if (timeUntilNextOffset <= 0)
        {
            timeUntilNextOffset = maxTimeUntilNextOffset;

            for (int i = 0; i < 3; i++)
            {
                if (deadEnemyIndex[i])
                { continue; }
                randomGuardOffset[i] = new Vector3(Random.Range(-randomOffsetRange, randomOffsetRange), 0, Random.Range(-randomOffsetRange, randomOffsetRange));
            }
        }

        for (int i = 0; i < 3; i++)
        {
            if (deadEnemyIndex[i])
            {
                continue; 
            }

            if (healthComponents[i].isDead)
            {
                guards[i].GetComponentInChildren<GuardIdleState>().SetFormationPosition(guards[i].transform.position);
                continue;
            }
            guards[i].GetComponentInChildren<GuardIdleState>().SetFormationPosition(enemyOffset[i] + playerPosition + randomGuardOffset[0]);
        }

        if (!isSomebodyAttacking) 
        {
            timeUntilNextAttack -= Time.deltaTime;

            if (timeUntilNextAttack <= 0)
            {
                if (guardScripts[nextGuardToAttack].CanAttack() && !deadEnemyIndex[nextGuardToAttack])
                {
                    guards[nextGuardToAttack].GetComponentInChildren<GuardChaseState>().player = playerObject.transform;
                    guardStateManagers[nextGuardToAttack].SwitchToNextState(guards[nextGuardToAttack].GetComponentInChildren<GuardChaseState>());
                    
                    timeUntilNextAttack = maxTimeUntilNextAttack + Random.Range(0, maxRandomTimeUntilNextOffset);
                }
                nextGuardToAttack++;
                nextGuardToAttack %= 3;
            }
        }
    }
}
