using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SusanooKnockbackState : State
{
    SusanooScript susanooScript;
    public SusanooIdleState idleState;
    float knockbackRemovePerHit = 0.25f;
    float knockbackTime = 1.25f;
    float maxKnockbackTime = 1.25f;

    // Start is called before the first frame update
    void Start()
    {
        susanooScript = GetComponentInParent<SusanooScript>();
    }

    public void SetKnockbackTime()
    {
        knockbackTime = maxKnockbackTime - (knockbackRemovePerHit * susanooScript.knockbackAmount);
    }

    public override State RunCurrentState()
    {
        // Show knockback anim
        // Does nothing

        knockbackTime -= Time.deltaTime;

        if (knockbackTime < 0)
        {
            Debug.Log("Knockback ended");
            return idleState;
        }

        return this;
    }
}
