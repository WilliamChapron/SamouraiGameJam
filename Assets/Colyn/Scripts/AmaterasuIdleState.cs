using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmaterasuIdleState : State
{
    bool playerInDetectionRange;
    AmaterasuScript amaterasu;
    public AmaterasuChaseState chaseState;

    void Start()
    {
        amaterasu = GetComponentInParent<AmaterasuScript>();
    }

    public override State RunCurrentState()
    {
        // Litteraly doesn't move until Player detection
        playerInDetectionRange = (transform.position - amaterasu.playerTransform.position).magnitude <= 8.0f;

        if (playerInDetectionRange)
        {
            return chaseState;
        }

        return this;
    }
}
