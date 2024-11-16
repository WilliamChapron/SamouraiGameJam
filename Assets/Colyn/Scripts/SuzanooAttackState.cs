using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuzanooAttackState : State
{
    EnemyStats enemyStats;
    SuzanooAttackRechargeState attackRechargeState;

    void Start()
    {
        enemyStats = GetComponentInParent<EnemyStats>();
    }

    public override State RunCurrentState()
    {
        // Attack
        Debug.Log("Attacks");
        enemyStats.Attack();



        return attackRechargeState;
    }
}
