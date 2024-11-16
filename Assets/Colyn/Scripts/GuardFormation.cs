using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardFormation : MonoBehaviour
{
    public bool isNobodyAttacking;

    public GameObject guardPrefab;

    GameObject[] guards = new GameObject[3];

    StateManager[] guardStateManagers = new StateManager[3];

    int nextGuardToAttack = 0;

    bool[] isGuardAttacking = new bool[3];

    GameObject playerObject;

    void Start()
    {
        guards[0] = Instantiate(guardPrefab, transform.position + new Vector3(-5, 0, 0), transform.rotation, transform);
        guards[1] = Instantiate(guardPrefab, transform.position + new Vector3(-2, 0, -5), transform.rotation, transform);
        guards[2] = Instantiate(guardPrefab, transform.position + new Vector3(-2, 0, 5), transform.rotation, transform);

        guardStateManagers[0] = guards[0].GetComponent<StateManager>();
        guardStateManagers[1] = guards[1].GetComponent<StateManager>();
        guardStateManagers[2] = guards[2].GetComponent<StateManager>();

        playerObject = GameObject.Find("Player");
    }

    private void Update()
    {
        Vector3 playerPosition = playerObject.transform.position;

        isGuardAttacking[0] = guardStateManagers[0].currentState.name == "AttackState";
        isGuardAttacking[1] = guardStateManagers[1].currentState.name == "AttackState";
        isGuardAttacking[2] = guardStateManagers[2].currentState.name == "AttackState";

        isNobodyAttacking = isGuardAttacking[0] || isGuardAttacking[1] || isGuardAttacking[2];

        guards[0].GetComponentInChildren<GuardIdleState>().SetFormationPosition(new Vector3(-5, 0, 0) + playerPosition);
        guards[1].GetComponentInChildren<GuardIdleState>().SetFormationPosition(new Vector3(-2, 0, -5) + playerPosition);
        guards[2].GetComponentInChildren<GuardIdleState>().SetFormationPosition(new Vector3(-2, 0, 5) + playerPosition);

        if (isNobodyAttacking)
        {
            guardStateManagers[nextGuardToAttack].SwitchToNextState(guards[nextGuardToAttack].GetComponentInChildren<GuardChaseState>());

            nextGuardToAttack++;
            nextGuardToAttack %= 3;
        }
    }

}
