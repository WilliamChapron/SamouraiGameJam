using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardScript : EnemyScript
{
    float rotationToY;

    public override void Start()
    {
        moveSpeed = 2.0f;

        maxAttackCooldown = 5.0f;

        base.Start();
    }

    public override void Update()
    {
        base.Update();

        // Turn to player
        //transform.LookAt(playerTransform);

        Vector3 lookDirection = playerTransform.position - transform.position;
        lookDirection.y = 0; // Gardez l'axe Y constant pour éviter de pencher vers le haut ou le bas
        transform.rotation = Quaternion.LookRotation(lookDirection);

        //transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
    }
}
