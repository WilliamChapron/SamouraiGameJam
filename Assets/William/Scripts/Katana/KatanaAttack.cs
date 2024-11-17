using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaAttack : MonoBehaviour
{
    public float damage = 10f;
    public bool isAttackActive = false;

    public bool isAttackON = false;
    public int hitNbr = 0;

    public void StartAttack()
    {
        isAttackActive = true;
    }

    public void StopAttack()
    {
        isAttackActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(gameObject.name);
        //Debug.Log(isAttackON);

        if (isAttackON && other.CompareTag("Boss"))
        {
            //Debug.Log("TRIGGER HIPS");
            //Debug.Log(other.gameObject.name);
            //Debug.Log(gameObject.name);

            BodyPartTest part = other.GetComponent<BodyPartTest>();
            
            if (part != null)
            {
                Debug.Log("Katana hit Boss : " + other.gameObject.name + " Hit Number: " + hitNbr);
                part.TakeDamage(damage);
                hitNbr += 1;
            }

        }
        if (isAttackON && other.CompareTag("Enemy"))
        {
            Debug.Log("Coup a l'enemy");
            Debug.Log(other.gameObject.name);
            HealthEnemiesComponent healthEnemiesComponent = other.GetComponent<HealthEnemiesComponent>();
            healthEnemiesComponent.TakeDamage(damage);
        }
    }
}
