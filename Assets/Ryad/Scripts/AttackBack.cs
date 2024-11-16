using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackBack : MonoBehaviour
{
    MoveBack move;

    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<MoveBack>();
    }

    // Update is called once per frame
    void Update()
    {
        if (move.isAttack)
        {
            Debug.Log("attack");
            move.isGoBack = false;
            move.isChase = false;
            move.isAttack = false;
            move.isEscape = true;
        }
    }
}
