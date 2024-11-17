using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboAttackBehaviour : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Attack attackComponent = animator.GetComponentInParent<Attack>();
        if (attackComponent != null)
        {
            attackComponent.IsComboAttacking = false;
            Debug.Log("IsComboAttacking r�initialis� � false.");
            attackComponent.katana1.StopAttack();
            attackComponent.katana2.StopAttack();
        }
        else
        {
            Debug.LogWarning("Composant Attack introuvable sur l'objet parent.");
        }
    }
}