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

    public Transform player;
    private NavMeshAgent agent;
    private Animator animator;

    private Vector3 destination;

    private bool HasWait = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        // Calcul initial de la position à droite du joueur
        SetRightPositionAsDestination();
    }

    void Update()
    {
        switch (state)
        {
            case 0:
                agent.isStopped = false;

                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        agent.SetDestination(player.position);
                        state = (state + 1) % nbState;
                    }
                }
                break;
            case 1:
                if (agent.remainingDistance <= distanceAttack)
                {
                    state = (state + 1) % nbState;
                }
                break;
            case 2:
                if (!HasWait)
                {
                    agent.isStopped = true;
                    HasWait = true;
                    break;
                }
                HasWait = false;
                animator.Play("Attack");
                
                Debug.Log("Attack");
                SetRightPositionAsDestination();
                agent.SetDestination(destination);

                state = (state + 1) % nbState;
                break;
        }
        transform.LookAt(player);

        Vector3  worldDeltaPosition = agent.destination - transform.position;

        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);

        animator.SetFloat("VelocityX", dx);
        animator.SetFloat("VelocityZ", dy);
    }

    private void SetRightPositionAsDestination()
    {
        Vector3 originalVector = player.position - transform.position; // Un vecteur sur l'axe X
        Quaternion rotation = Quaternion.Euler(0, 90, 0); // Rotation de 90 degrés autour de l'axe Y
        Vector3 rotatedVector = rotation * originalVector;
        int nbRandom = Random.Range(0, 2);
        float distance = (float)Random.Range(12, (int)(distanceEscape * 10) + 1) / 10f;
        int direction;
        if (nbRandom == 0)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }
        destination = player.position + rotatedVector * distance * direction;
    }
}
