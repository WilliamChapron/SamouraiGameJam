using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveBack : MonoBehaviour
{
    [SerializeField] int distanceEscape;
    [SerializeField] int distanceAttack;

    public bool isGoBack = true;  
    public bool isChase = false;
    public bool isAttack = false;
    public bool isEscape = false;
    
    public Transform player;
    private NavMeshAgent agent;

    private Vector3 destination;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(player.position);

        Vector3 vector = new Vector3(3f, 0f, 3f);
        destination = player.position + vector;
        agent.SetDestination(destination);
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
        if (isGoBack)
        {
            agent.isStopped = false;
            agent.SetDestination(destination);
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    Debug.Log("L'agent a atteint sa destination.");
                    isGoBack = false;
                    isChase = true;
                }
            }
        }
        else if (isChase)
        {
            agent.SetDestination(player.position);
            if (agent.remainingDistance > distanceAttack)
            {
                Debug.Log("Chase en cours...");
                return;
            }
            else
            {
                isChase = false;
                isAttack = true;
                Debug.Log("Transition vers l'attaque.");
            }
        }
        else if (isAttack)
        {
            if (!agent.isStopped)
            {
                agent.isStopped = true;
                Debug.Log("L'agent s'arrête pour attaquer.");
                return;
            }
            else
            {
                //agent.isStopped = true;
                Debug.Log("attack");

                Vector3 vector = new Vector3(3f, 0f, 3f);
                destination = player.position + vector;

                agent.SetDestination(destination);


                isAttack = false;
                isGoBack = true;

                Debug.Log("L'enemi passe par deriere");
            }
        }
    }
}
