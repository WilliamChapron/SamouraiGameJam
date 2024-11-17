using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class behaviourEnemy2 : MonoBehaviour
{
    [SerializeField] float distanceEscape;
    [SerializeField] float distanceAttack;

    private int nbState = 3;
    [Range(0, 2)][SerializeField] int state = 0;

    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private AttackEnemies attackEnemy;
    private HealthEnemiesComponent healthEnemy;

    private Vector3 destination;

    private bool hasWaited = false;

    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player not found. Please ensure an object with the tag 'Player' exists in the scene.");
        }

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        attackEnemy = GetComponent<AttackEnemies>();
        healthEnemy = GetComponent<HealthEnemiesComponent>();

        // Initial calculation of the position to the right of the player
        SetRightPositionAsDestination();
    }

    void Update()
    {
        if (healthEnemy.isDead)
        {
            agent.isStopped = true;
            return;
        }

        switch (state)
        {
            case 0: // GoBack
                HandleGoBackState();
                break;
            case 1: // Chase
                HandleChaseState();
                break;
            case 2: // Wait and Attack
                if (!hasWaited)
                {
                    StartCoroutine(WaitAndAttackCoroutine());
                }
                break;
        }

        UpdateAnimationParameters();
    }

    private void HandleGoBackState()
    {
        agent.isStopped = false;
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                agent.SetDestination(player.position);
                state = (state + 1) % nbState;
            }
        }
    }

    private void HandleChaseState()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
        if (agent.remainingDistance <= distanceAttack)
        {
            state = (state + 1) % nbState;
        }
    }

    private IEnumerator WaitAndAttackCoroutine()
    {
        hasWaited = true;
        agent.isStopped = true;

        attackEnemy.StartAttack();

        animator.SetTrigger("Random1");

        yield return new WaitForSeconds(3.2f);

        SetRightPositionAsDestination();
        agent.SetDestination(destination);
        hasWaited = false;
        state = (state + 1) % nbState;
    }

    private void UpdateAnimationParameters()
    {
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

    private void SetRightPositionAsDestination()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        Quaternion rotation = Quaternion.Euler(0, 90, 0);
        Vector3 rotatedVector = rotation * directionToPlayer;
        float distance = Random.Range(1.2f, distanceEscape);
        int direction = Random.value < 0.5f ? -1 : 1;

        destination = player.position + rotatedVector * distance * direction;
    }
}
