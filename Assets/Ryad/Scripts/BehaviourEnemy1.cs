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

    private int nbState = 5;
    [Range(0, 4)][SerializeField] int state = 0;

    public Transform player;
    private NavMeshAgent agent;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(player.position);
        animator = GetComponentInChildren<Animator>();
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
            case 0:
                agent.SetDestination(player.position);
                if (agent.remainingDistance <= distanceAttack)
                {
                    state = (state + 1) % nbState;
                }
                break;
            case 1:
                agent.SetDestination(player.position);

                if (agent.remainingDistance <= distanceAttack)
                {
                    state = (state + 1) % nbState;
                }
                break;
            case 2:
                if (!agent.isStopped)
                {
                    agent.isStopped = true;
                }

                Debug.Log("attack");
                animator.Play("Attack");

                state = (state + 1) % nbState;
                break;
            case 3:
                Vector3 lookDirection = player.position - transform.position;
                lookDirection.y = 0; // Gardez l'axe Y constant pour éviter de pencher vers le haut ou le bas
                transform.rotation = Quaternion.LookRotation(lookDirection);

                Debug.Log("L'ennemi entre en mode fuite.");
                Escape();
                cooldown = cooldownMax;
                state = (state + 1) % nbState;
                break;
            case 4:
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
