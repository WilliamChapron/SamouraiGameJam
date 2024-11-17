using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BehaviourEnemy1 : MonoBehaviour
{
    [SerializeField] float distanceEscape;
    [SerializeField] float distanceAttack;
    [SerializeField] int cooldownMax;
    [SerializeField] float escapeDelay = 3f; // Délai avant de reculer
    private float cooldown;

    private int nbState = 4;
    [Range(0, 3)][SerializeField] int state = 0;

    public Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private AttackEnemies attackEnemy;
    private bool isEscaping = false; // Pour vérifier si le délai est actif

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

                attackEnemy.StartAttack();

                if (Random.Range(0, 2) == 0)
                {
                    animator.SetTrigger("Random1");
                }
                else
                {
                    animator.SetTrigger("Random2");
                }

                state = (state + 1) % nbState;
                break;

            case 2: // escape
                if (!isEscaping)
                {
                    StartCoroutine(DelayedEscape());
                }
                break;

            case 3: // wait
                if (cooldown > 0)
                {
                    cooldown -= Time.deltaTime;
                    if (cooldown <= 0)
                    {
                        state = (state + 1) % nbState;
                        agent.SetDestination(player.position);
                    }
                }
                break;
        }

        Vector3 lookPosition = player.position - transform.position;
        lookPosition.y = 0;

        if (lookPosition != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(lookPosition);
        }

        Vector3 worldDeltaPosition = agent.destination - transform.position;

        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);

        animator.SetFloat("VelocityX", dx);
        animator.SetFloat("VelocityZ", dy);
    }

    IEnumerator DelayedEscape()
    {
        isEscaping = true;
        yield return new WaitForSeconds(escapeDelay);
        Escape();
        cooldown = cooldownMax;
        state = (state + 1) % nbState;
        isEscaping = false;
    }
}
