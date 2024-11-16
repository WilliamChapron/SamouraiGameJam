using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Attack : MonoBehaviour
{
    Move move;

    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<Move>();
    }

    // Update is called once per frame
    void Update()
    {
        if (move.isAttack)
        {
            Debug.Log("attack");
            move.isChase = false;
            move.isAttack = false;
            move.isEscape = true;
        }
    }
}
