using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaAttack : MonoBehaviour
{
    public float damage = 10f;
    public bool isAttackActive = false;
    public int hitNbr = 0;

    // Appelée pour démarrer l'attaque
    public void StartAttack()
    {
        isAttackActive = true;
        Debug.Log("Katana attack activated.");
    }

    // Appelée pour arrêter l'attaque
    public void StopAttack()
    {
        isAttackActive = false;
        Debug.Log("Katana attack deactivated.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAttackActive && other.CompareTag("Boss"))
        {
            Debug.Log("On trigger");
            Boss boss = other.GetComponent<Boss>();
            if (boss != null)
            {
                Debug.Log("Katana hit Boss : " + other.gameObject.name + " Hit Number: " + hitNbr);
                boss.TakeDammage(damage);
                hitNbr += 1;
            }
        }
    }
}
