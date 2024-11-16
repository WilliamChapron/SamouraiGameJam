using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardScript : EnemyScript
{
    float rotationToX;
    float rotationToY;

    public override void Start()
    {
        moveSpeed = 3.0f;

        maxAttackCooldown = 5.0f;

        base.Start();
    }

    private void Update()
    {
        // Turn to player

        rotationToX = Quaternion.FromToRotation(transform.position, playerTransform.position).x;
        rotationToY = Quaternion.FromToRotation(transform.position, playerTransform.position).y;
        //transform.rotation.x = Mathf.LerpAngle(rotationToX, transform.rotation.x, 0.1f);
    }

}
