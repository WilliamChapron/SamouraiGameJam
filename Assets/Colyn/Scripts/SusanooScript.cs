using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SusanooScript : EnemyScript
{
    // Start is called before the first frame update
    public override void Start()
    {
        moveSpeed = 3.0f;

        maxAttackCooldown = 5.0f;

        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
}
