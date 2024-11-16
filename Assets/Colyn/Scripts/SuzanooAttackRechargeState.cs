using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuzanooAttackRechargeState : State
{
    EnemyStats enemyStats;
    public SuzanooChaseState chaseState;

    public override State RunCurrentState()
    {
        if (enemyStats.CanAttack())
        {
            return chaseState;
        }

        return this; 
    }
}
