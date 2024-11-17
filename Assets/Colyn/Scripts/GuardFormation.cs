using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardFormation : MonoBehaviour
{
    public bool isSomebodyAttacking;

    public GameObject guardPrefab;

    GameObject[] guards = new GameObject[5];

    StateManager[] guardStateManagers = new StateManager[5];

    int nextGuardToAttack = 0;

    bool[] isGuardAttacking = new bool[5];

    GuardScript[] guardScripts = new GuardScript[5];

    HealthEnemiesComponent[] healthComponents = new HealthEnemiesComponent[5];

    GameObject playerObject;

    float maxTimeUntilNextAttack = 0.5f;

    float timeUntilNextAttack = 0.5f;

    float maxTimeUntilNextOffset = 1.5f;

    float timeUntilNextOffset = 1.5f;

    float maxRandomTimeUntilNextOffset = 1f;

    Vector3[] randomGuardOffset = new Vector3[5];

    float randomOffsetRange = 0.8f;

    bool[] deadEnemyIndex = new bool[5];

    Vector3[] enemyOffset = new Vector3[5];
    Vector3 generalFormationOffset;
    Vector3[] enemySpawn = new Vector3[5];

    void Start()
    {
        playerObject = GameObject.Find("Player");
        Vector3 playerPosition = playerObject.transform.position;

        enemyOffset[0] = new Vector3(-8, 0, 12);  // Gauche élargie (Rangée 1)
        enemyOffset[1] = new Vector3(8, 0, 12);   // Droite élargie (Rangée 1)

        enemyOffset[2] = new Vector3(-4, 0, 6);  // Gauche rapprochée (Rangée 2)
        enemyOffset[3] = new Vector3(4, 0, 6);   // Droite rapprochée (Rangée 2)

        enemyOffset[4] = new Vector3(0, 0, 0);   // Centre bas (Rangée 3)

        enemySpawn[0] = new Vector3(-16, 0, 24);  // Gauche élargie (Rangée 1)
        enemySpawn[1] = new Vector3(16, 0, 24);   // Droite élargie (Rangée 1)

        enemySpawn[2] = new Vector3(-8, 0, 12);   // Gauche rapprochée (Rangée 2)
        enemySpawn[3] = new Vector3(8, 0, 12);    // Droite rapprochée (Rangée 2)

        enemySpawn[4] = new Vector3(0, 0, 0);     // Centre bas (Rangée 3)

        generalFormationOffset = new Vector3(0, 0, -6);

        for (int i = 0; i < 5; i++)
        {
            deadEnemyIndex[i] = false;
            guards[i] = Instantiate(guardPrefab, playerPosition + enemySpawn[i] + (generalFormationOffset * 2), transform.rotation, transform);
            guardStateManagers[i] = guards[i].GetComponent<StateManager>();
            guardScripts[i] = guards[i].GetComponent<GuardScript>();
            healthComponents[i] = guards[i].GetComponent<HealthEnemiesComponent>();
        }    
    }

    private void Update()
    {
        Vector3 playerPosition = playerObject.transform.position;

        for (int i = 0; i < 5; i++)
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

            for (int i = 0; i < 5; i++)
            {
                if (deadEnemyIndex[i])
                { continue; }
                randomGuardOffset[i] = new Vector3(Random.Range(-randomOffsetRange, randomOffsetRange), 0, Random.Range(-randomOffsetRange, randomOffsetRange));
            }
        }

        for (int i = 0; i < 5; i++)
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
            guards[i].GetComponentInChildren<GuardIdleState>().SetFormationPosition(enemyOffset[i] + playerPosition + randomGuardOffset[i] + generalFormationOffset);
        }

        if (!isSomebodyAttacking) 
        {
            timeUntilNextAttack -= Time.deltaTime;

            if (timeUntilNextAttack <= 0)
            {
                if (guardScripts[nextGuardToAttack].CanAttack() && !deadEnemyIndex[nextGuardToAttack])
                {
                    guardScripts[nextGuardToAttack].Accelerate();
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
