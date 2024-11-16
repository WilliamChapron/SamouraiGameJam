using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SusanooAttackState : State
{
    SusanooScript susanooScript;
    public SusanooAttackRechargeState attackRechargeState;

    void Start()
    {
        susanooScript = GetComponentInParent<SusanooScript>();
    }

    public override State RunCurrentState()
    {
        // Attack
        Debug.Log("Attacks");
        susanooScript.Attack();

        return attackRechargeState;
    }
}
