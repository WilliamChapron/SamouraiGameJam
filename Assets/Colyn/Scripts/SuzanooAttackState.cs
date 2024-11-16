using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuzanooAttackState : State
{
    EnemyScript enemyStats;
    SuzanooAttackRechargeState attackRechargeState;

    void Start()
    {
        enemyStats = GetComponentInParent<EnemyScript>();
    }

    public override State RunCurrentState()
    {
        // Attack
        Debug.Log("Attacks");
        enemyStats.Attack();



        return attackRechargeState;
    }
}
