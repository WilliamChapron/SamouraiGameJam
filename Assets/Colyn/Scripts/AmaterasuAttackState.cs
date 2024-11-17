using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmaterasuAttackState : State
{
    AmaterasuScript amaterasu;
    public AmaterasuAttackRechargeState rechargeState;

    void Start()
    {
        amaterasu = GetComponentInParent<AmaterasuScript>();
    }

    public override State RunCurrentState()
    {
        amaterasu.agent.destination = amaterasu.transform.position;

        // One big hit into 2-3 seconds of Attack Recharge State

        amaterasu.Attack();

        return rechargeState;
    }

}
