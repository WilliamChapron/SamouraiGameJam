using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SusanooIdleState : State
{
    SusanooScript susanooScript;
    public bool playerInDetectionRange;
    public SusanooChaseState chaseState;
    public SusanooAttackRechargeState attackRechargeState;

    void Start()
    {
        susanooScript = GetComponentInParent<SusanooScript>();
    }

    public override State RunCurrentState()
    {
        playerInDetectionRange = (transform.position - susanooScript.playerTransform.position).magnitude <= 12.0f;

        if (playerInDetectionRange)
        {
            if(susanooScript.CanAttack())
            {
                return chaseState;
            }

            return attackRechargeState;
        }

        return this;
    }

}
