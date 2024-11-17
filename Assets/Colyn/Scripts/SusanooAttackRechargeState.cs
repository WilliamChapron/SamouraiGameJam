using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SusanooAttackRechargeState : State
{
    SusanooScript susanooScript;
    public SusanooChaseState chaseState;

    void Start()
    {
        susanooScript = GetComponentInParent<SusanooScript>();
    }

    public override State RunCurrentState()
    {
        if (susanooScript.CanAttack())
        {
            return chaseState;
        }

        return this; 
    }
}
