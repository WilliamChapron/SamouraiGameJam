using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public AttackCollider attackCollider;

    public float maxAttackCooldown;
    public float attackCooldown = 0;

    public int damage;
    public float moveSpeed;

    public NavMeshAgent agent;

    GameObject playerObject;
    public Transform playerTransform;

    protected Animator animator;

    public HealthComponent healthComponent;

    public virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();

        playerObject = GameObject.Find("Player");
        playerTransform = playerObject.transform;

        healthComponent = GetComponent<HealthComponent>();
        attackCollider = GetComponentInChildren<AttackCollider>();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
    }

    public virtual void Update()
    {
        attackCooldown -= Time.deltaTime;
    }

    public virtual void TakeDamage(int damage)
    {
        Debug.Log("Has been hit");
        animator.SetTrigger("Hit");
        healthComponent.TakeDamage(damage);
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
