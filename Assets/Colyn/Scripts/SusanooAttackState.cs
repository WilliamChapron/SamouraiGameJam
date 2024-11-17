using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SusanooAttackState : State
{
    SusanooScript susanooScript;
    public SusanooAttackRechargeState attackRechargeState;

    float attackTime = 0.5f;
    float maxAttackTime = 0.5f;

    void Start()
    {
        susanooScript = GetComponentInParent<SusanooScript>();
    }

    public override State RunCurrentState()
    {
        // Attack
        Debug.Log("Attacks");
        susanooScript.PlayAttack();

        attackTime -= Time.deltaTime;

        if (attackTime < 0)
        {
            attackTime = maxAttackTime;
            return attackRechargeState;
        }

        return this;
    }
}
