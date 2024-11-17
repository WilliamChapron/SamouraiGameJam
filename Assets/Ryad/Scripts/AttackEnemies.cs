using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemies : MonoBehaviour
{
    [SerializeField] float damage;


    public void Attack()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.GetComponent<HealthComponent>()?.TakeDamage(damage);
        }
    }
}
