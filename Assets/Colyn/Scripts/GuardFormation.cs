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

    GameObject playerObject;

    float maxTimeUntilNextAttack = 2.0f;

    float timeUntilNextAttack = 2.0f;

    void Start()
    {
        guards[0] = Instantiate(guardPrefab, transform.position + new Vector3(-5, 0, 0), transform.rotation, transform);
        guards[1] = Instantiate(guardPrefab, transform.position + new Vector3(-2, 0, -5), transform.rotation, transform);
        guards[2] = Instantiate(guardPrefab, transform.position + new Vector3(-2, 0, 5), transform.rotation, transform);

        guardStateManagers[0] = guards[0].GetComponent<StateManager>();
        guardStateManagers[1] = guards[1].GetComponent<StateManager>();
        guardStateManagers[2] = guards[2].GetComponent<StateManager>();

        guardScripts[0] = guards[0].GetComponent<GuardScript>();
        guardScripts[1] = guards[1].GetComponent<GuardScript>();
        guardScripts[2] = guards[2].GetComponent<GuardScript>();

        playerObject = GameObject.Find("Player");
    }

    private void Update()
    {
        Vector3 playerPosition = playerObject.transform.position;

        isGuardAttacking[0] = guardStateManagers[0].currentState.name == "ChaseState";
        isGuardAttacking[1] = guardStateManagers[1].currentState.name == "ChaseState";
        isGuardAttacking[2] = guardStateManagers[2].currentState.name == "ChaseState";

        isSomebodyAttacking = isGuardAttacking[0] || isGuardAttacking[1] || isGuardAttacking[2];

        guards[0].GetComponentInChildren<GuardIdleState>().SetFormationPosition(new Vector3(-5, 0, 0) + playerPosition);
        guards[1].GetComponentInChildren<GuardIdleState>().SetFormationPosition(new Vector3(-2, 0, -5) + playerPosition);
        guards[2].GetComponentInChildren<GuardIdleState>().SetFormationPosition(new Vector3(-2, 0, 5) + playerPosition);

        

        // To do: Add more time between attacks
        if (!isSomebodyAttacking) 
        {
            timeUntilNextAttack -= Time.deltaTime;

            if (timeUntilNextAttack <= 0)
            {

                if (guardScripts[nextGuardToAttack].CanAttack())
                {
                    guards[nextGuardToAttack].GetComponentInChildren<GuardChaseState>().player = playerObject.transform;
                    guardStateManagers[nextGuardToAttack].SwitchToNextState(guards[nextGuardToAttack].GetComponentInChildren<GuardChaseState>());

                    Debug.Log("Attack ran");

                    timeUntilNextAttack = maxTimeUntilNextAttack;
                }

                Debug.Log("Changes attacker");
                nextGuardToAttack++;
                nextGuardToAttack %= 3;
            }
        }
    }
}
