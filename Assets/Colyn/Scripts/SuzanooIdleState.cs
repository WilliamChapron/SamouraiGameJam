using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuzanooIdleState : State
{
    EnemyScript enemyStats;
    public bool playerInSight;
    public SuzanooChaseState chaseState;
    public SuzanooAttackRechargeState attackRechargeState;

    void Start()
    {
        enemyStats = GetComponentInParent<EnemyScript>();
    }

    public override State RunCurrentState()
    {
        if(playerInSight)
        {
            if(enemyStats.CanAttack())
            {
                return chaseState;
            }

            return attackRechargeState;
        }


        return this;
    }

}
