using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttackBehaviour : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Attack attackComponent = animator.GetComponentInParent<Attack>();
        if (attackComponent != null)
        {
            attackComponent.IsLightAttacking = false;
            //Debug.Log("IsLightAttacking réinitialisé à false.");
            attackComponent.katana1.StopAttack();
            attackComponent.katana2.StopAttack();

            attackComponent.katana1.isAttackON = false;
            attackComponent.katana2.isAttackON = false;

        }
        else
        {
            Debug.LogWarning("Composant Attack introuvable sur l'objet parent.");
        }
    }
}