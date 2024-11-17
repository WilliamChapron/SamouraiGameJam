using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardKnockbackState : State
{
    GuardScript guard;
    public GuardIdleState idleState;
    float knockbackRemovePerHit = 0.25f;
    float knockbackTime = 0.75f;
    float maxKnockbackTime = 0.75f;

    // Start is called before the first frame update
    void Start()
    {
        guard = GetComponentInParent<GuardScript>();
    }

    public void SetKnockbackTime()
    {
        knockbackTime = maxKnockbackTime - (knockbackRemovePerHit * guard.knockbackAmount);
    }


    public override State RunCurrentState()
    {
        // Show knockback anim
        // Does nothing
        guard.agent.destination = transform.position;

        knockbackTime -= Time.deltaTime;

        if (knockbackTime < 0)
        {
            Debug.Log("Knockback ended");
            return idleState;
        }

        return this;
    }
}
