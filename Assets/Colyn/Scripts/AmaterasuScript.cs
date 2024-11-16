using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmaterasuScript : EnemyScript
{



    public override void Start()
    {
        moveSpeed = 1.0f;

        maxAttackCooldown = 4.0f;

        base.Start();
    }

}
