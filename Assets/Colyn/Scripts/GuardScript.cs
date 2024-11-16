using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardScript : EnemyScript
{
    float rotationToY;
    public int knockbackAmount = 0;

    float maxTimeUntilKnockbackReset = 1.5f;
    float timeUntilKnockbackReset = 1.5f;

    public GuardKnockbackState knockbackState;
    public StateManager stateManager;

    public override void Start()
    {
        stateManager = GetComponent<StateManager>();

        moveSpeed = 2.0f;

        maxAttackCooldown = 5.0f;

        base.Start();
    }

    public override void TakeHit(int damage)
    {
        timeUntilKnockbackReset = maxTimeUntilKnockbackReset;

        knockbackAmount++;

        knockbackState.SetKnockbackTime();
        stateManager.SwitchToNextState(knockbackState);

        base.TakeHit(damage);
    }

    public override void Update()
    {
        base.Update();

        // Turn to player
        //transform.LookAt(playerTransform);

        Vector3 lookDirection = playerTransform.position - transform.position;
        lookDirection.y = 0; // Gardez l'axe Y constant pour éviter de pencher vers le haut ou le bas
        transform.rotation = Quaternion.LookRotation(lookDirection);

        timeUntilKnockbackReset -= Time.deltaTime;

        if (knockbackAmount != 0  && timeUntilKnockbackReset <= 0)
        {
            knockbackAmount = 0;
            Debug.Log("Knockback Reset");
        }

        //transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
    }
}
