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

    protected Animator animator;

    public virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();

        playerObject = GameObject.Find("Player");
        playerTransform = playerObject.transform;

        attackCollider = GetComponentInChildren<AttackCollider>();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
    }

    public virtual void Update()
    {
        attackCooldown -= Time.deltaTime;
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    public virtual void TakeHit(int damage)
    {
        currentHealth -= damage;

        if(IsDead())
        {


            // Play death anim
        }
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

        animator.Play("Attack");

        // Run this in animation

        // If collision with attack hitbox, deal damage
        if (attackCollider.playerCollides)
        {
            //Player.TakeDamage(damage);
        }
        
        // Run this here

        SetAttackCooldown();
    }
}
