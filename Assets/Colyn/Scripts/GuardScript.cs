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

        healthComponent.maxHealth = 50.0f;
    }

    public override void TakeDamage(int damage)
    {
        timeUntilKnockbackReset = maxTimeUntilKnockbackReset;

        knockbackAmount++;

        knockbackState.SetKnockbackTime();
        stateManager.SwitchToNextState(knockbackState);



        base.TakeDamage(damage);
    }

    public override void Update()
    {
        base.Update();

        // Turn to player

        Vector3 lookDirection = playerTransform.position - transform.position;
        lookDirection.y = 0; // Gardez l'axe Y constant pour �viter de pencher vers le haut ou le bas
        transform.rotation = Quaternion.LookRotation(lookDirection);

        timeUntilKnockbackReset -= Time.deltaTime;

        if (knockbackAmount != 0  && timeUntilKnockbackReset <= 0)
        {
            knockbackAmount = 0;
            Debug.Log("Knockback Reset");
        }

        Vector3 worldDeltaPosition = agent.destination - transform.position;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);

        animator.SetFloat("VelocityX", dx);
        animator.SetFloat("VelocityZ", dy);
    }
}
