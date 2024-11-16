using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour { }

public class EnemyStats : MonoBehaviour
{
    float maxAttackCooldown;
    public float attackCooldown = 0;
    int maxHealth;
    public int currentHealth;

    public int damage;
    public float moveSpeed;
    
    void SetAttackCooldown()
    {
        attackCooldown = maxAttackCooldown;
    }

    public bool CanAttack()
    {
        return attackCooldown <= 0.0f;
    }

    public void Attack()
    {
        // Play attack animation
        // If collision with attack hitbox, deal damage

        SetAttackCooldown();
    }
}
