using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardScript : EnemyScript
{
    float rotationToY;

    public override void Start()
    {
        moveSpeed = 3.0f;

        maxAttackCooldown = 5.0f;

        base.Start();
    }

    public override void Update()
    {
        base.Update();

        // Turn to player
        transform.LookAt(playerTransform);

        //transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
    }
}
