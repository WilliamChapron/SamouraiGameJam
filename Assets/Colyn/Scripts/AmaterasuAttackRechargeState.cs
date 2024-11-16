using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmaterasuAttackRechargeState : State
{
    AmaterasuScript amaterasu;
    public AmaterasuChaseState chaseState;

    void Start()
    {
        amaterasu = GetComponentInParent<AmaterasuScript>();
    }

    public override State RunCurrentState()
    {
        // Doesn't move.

        if(amaterasu.CanAttack())
        {
            return chaseState;
        }

        return this;
    }
}
