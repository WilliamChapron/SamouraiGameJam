using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public AttackCollider attackCollider;

    public float maxAttackCooldown;
    public float attackCooldown = 0;
    int maxHealth;
    public int currentHealth;

    public int damage;
    public float moveSpeed;

    public NavMeshAgent agent;

    GameObject playerObject;
    public Transform playerTransform;

    public virtual void Start()
    {
        playerObject = GameObject.Find("Player");
        playerTransform = playerObject.transform;

        attackCollider = GetComponentInChildren<AttackCollider>();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
    }

    private void Update()
    {
        attackCooldown -= Time.deltaTime;
    }

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

        // Run this in animation

        // If collision with attack hitbox, deal damage
        if(attackCollider.playerCollides)
        {
            //Player.TakeDamage(damage);
        }
        
        // Run this here

        SetAttackCooldown();
    }
}
