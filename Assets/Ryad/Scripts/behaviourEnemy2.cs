using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class behaviourEnemy2 : MonoBehaviour
{
    [SerializeField] float distanceEscape;
    [SerializeField] float distanceAttack;
    [SerializeField] int cooldownMax;
    private float cooldown;

    private int nbState = 4;
    [Range(0, 4)][SerializeField] int state = 0;

    public Transform player;
    private NavMeshAgent agent;

    private Vector3 destination;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

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
                        state = (state + 1) % nbState;
                    }
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
                agent.isStopped = true;
                Debug.Log("Attack");

                cooldown = cooldownMax;
                state = (state + 1) % nbState;
                break;
            case 3:
                if (cooldown > 0)
                {
                    cooldown -= Time.deltaTime;
                    Debug.Log("Wait");
                    if (cooldown <= 0)
                    {
                        state = (state + 1) % nbState;

                        SetRightPositionAsDestination();
                        agent.SetDestination(destination);
                    }
                }
                break;
        }
    }

    private void SetRightPositionAsDestination()
    {
        Vector3 originalVector = player.position - transform.position; // Un vecteur sur l'axe X
        Quaternion rotation = Quaternion.Euler(0, 90, 0); // Rotation de 90 degrés autour de l'axe Y
        Vector3 rotatedVector = rotation * originalVector;
        destination = player.position + rotatedVector * distanceEscape;
    }
}
