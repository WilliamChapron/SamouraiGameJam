using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SusanooScript : EnemyScript
{
    public int knockbackAmount = 0;

    float maxTimeUntilKnockbackReset = 1.5f;
    float timeUntilKnockbackReset = 1.5f;

    public SusanooKnockbackState knockbackState;
    public StateManager stateManager;

    public override void Start()
    {
        stateManager = GetComponent<StateManager>();

        moveSpeed = 3.0f;

        maxAttackCooldown = 5.0f;

        base.Start();
    }

    //public override void TakeHit(int damage)
    //{
    //    timeUntilKnockbackReset = maxTimeUntilKnockbackReset;

    //    knockbackAmount++;

    //    knockbackState.SetKnockbackTime();
    //    stateManager.SwitchToNextState(knockbackState);

    //    base.TakeHit(damage);
    //}

    public override void Update()
    {
        base.Update();
    }
}
