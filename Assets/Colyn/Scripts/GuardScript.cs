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

    int VelocityXHash;
    int VelocityZHash;

    public override void Start()
    {
        stateManager = GetComponent<StateManager>();

        moveSpeed = 2.0f;

        maxAttackCooldown = 5.0f;

        VelocityXHash = Animator.StringToHash("VelocityX");
        VelocityZHash = Animator.StringToHash("VelocityZ");

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

        Vector3 worldDeltaPosition = agent.destination - transform.position;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        Vector3 walkVector = (transform.position - agent.destination);
        animator.SetFloat(VelocityXHash, dx);
        animator.SetFloat(VelocityZHash, dy);
    }
}
