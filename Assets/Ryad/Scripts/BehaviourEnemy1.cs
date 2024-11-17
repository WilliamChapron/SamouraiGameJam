using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BehaviourEnemy1 : MonoBehaviour
{
    [SerializeField] float distanceEscape;
    [SerializeField] float distanceAttack;
    [SerializeField] int cooldownMax;
    private float cooldown;

    private int nbState = 4;
    [Range(0, 3)][SerializeField] int state = 0;

    public Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private AttackEnemies attackEnemy;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(player.position);
        animator = GetComponentInChildren<Animator>();
        attackEnemy = GetComponent<AttackEnemies>();
    }


    void Escape()
    {
        Vector3 direction = (transform.position - player.position).normalized;
        Vector3 destination = transform.position + direction * distanceEscape;
        agent.SetDestination(destination);
        agent.isStopped = false;

        Debug.Log("L'ennemi s'éloigne du joueur.");
    }

    void Update()
    {
        switch (state)
        {
            case 0: // chase
                agent.SetDestination(player.position);
                if (agent.remainingDistance <= distanceAttack)
                {
                    state = (state + 1) % nbState;
                }
                break;
            case 1: // attack
                if (!agent.isStopped)
                {
                    agent.isStopped = true;
                }

                Debug.Log("attack");
                attackEnemy.Attack();
                animator.Play("Attack");

                state = (state + 1) % nbState;
                break;
            case 2: // escape
                Vector3 lookDirection = player.position - transform.position;
                lookDirection.y = 0;
                transform.rotation = Quaternion.LookRotation(lookDirection);

                Debug.Log("L'ennemi entre en mode fuite.");
                Escape();
                cooldown = cooldownMax;
                state = (state + 1) % nbState;
                break;
            case 3: // wait
                if (cooldown > 0)
                {
                    cooldown -= Time.deltaTime;
                    if (cooldown <= 0)
                    {
                        state = (state + 1) % nbState;
                        agent.SetDestination(player.position);
                        Debug.Log("Le cooldown est terminé, l'ennemi reprend la poursuite.");
                    }
                }
                break;
        }
        transform.LookAt(player);

        Vector3 worldDeltaPosition = agent.destination - transform.position;

        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);

        animator.SetFloat("VelocityX", dx);
        animator.SetFloat("VelocityZ", dy);
    }
}
