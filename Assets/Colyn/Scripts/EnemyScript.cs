using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    
    public AttackCollider attackCollider;
    private CapsuleCollider playerCollider;

    public float maxAttackCooldown;
    public float attackCooldown = 0;

    public float damage;
    public float moveSpeed;

    public NavMeshAgent agent;

    protected GameObject playerObject;
    public Transform playerTransform;

    protected Animator animator;

    public HealthEnemiesComponent healthComponent;

    public virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();

        playerObject = GameObject.Find("Player");
        playerTransform = playerObject.transform;

        healthComponent = GetComponent<HealthEnemiesComponent>();
        attackCollider = GetComponentInChildren<AttackCollider>();
        playerCollider = playerObject.GetComponent<CapsuleCollider>();

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

    public void SetAttackCooldown()
    {
        attackCooldown = maxAttackCooldown;
    }

    public bool CanAttack()
    {
        return attackCooldown <= 0.0f;
    }

    public void PlayAttack()
    {
        // Play attack animation

        animator.Play("Attack");

        SetAttackCooldown();
    }
}
