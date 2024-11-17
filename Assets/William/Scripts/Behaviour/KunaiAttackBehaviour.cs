using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiAttackBehaviour : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Attack attackComponent = animator.GetComponentInParent<Attack>();
        if (attackComponent != null)
        {
            attackComponent.IsKunaiAttacking = false;
            //Debug.Log("IsLightAttacking réinitialisé à false.");
        }
        else
        {
            Debug.LogWarning("Composant Attack introuvable sur l'objet parent.");
        }
    }
}