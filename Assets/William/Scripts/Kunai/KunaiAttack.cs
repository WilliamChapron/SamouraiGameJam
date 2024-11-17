using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiAttack : MonoBehaviour
{
    public int damage = 10;

    private void OnTriggerEnter(Collider other)
    {
            if (other.CompareTag("Boss"))
            {
                Debug.Log("Kunai hit Boss : " + other.gameObject.name);

                BodyPartTest part = other.GetComponent<BodyPartTest>();
                if (part != null)
                {
                    part.TakeDamage(damage);
                }
                Destroy(gameObject);
            }
            else if (other.CompareTag("Enemy"))
            {
                Debug.Log("Kunai hit Enemy : " + other.gameObject.name);

                HealthEnemiesComponent healthEnemiesComponent = other.GetComponent<HealthEnemiesComponent>();
                if (healthEnemiesComponent != null)
                {
                    healthEnemiesComponent.TakeDamage(damage);
                }
                Destroy(gameObject);
            }

        }
}