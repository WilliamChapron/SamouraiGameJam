using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Move : MonoBehaviour
{
    [SerializeField] int distanceEscape;
    [SerializeField] int distanceAttack;
    [SerializeField] int cooldownMax;
    private float cooldown;

    public bool isAttack = false;
    public bool isEscape = false;
    public bool isChase = true;
    public Transform player;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(player.position);
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
        if (isChase)
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
            }
        }

        else if (isEscape)
        {
            Vector3 lookDirection = player.position - transform.position;
            lookDirection.y = 0; // Gardez l'axe Y constant pour éviter de pencher vers le haut ou le bas
            transform.rotation = Quaternion.LookRotation(lookDirection);

            Debug.Log("L'ennemi entre en mode fuite.");
            Escape();
            cooldown = cooldownMax;
            isEscape = false;
        }
        else
        {
            transform.LookAt(player);
        }

        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            if (cooldown <= 0)
            {
                isChase = true;
                agent.SetDestination(player.position);
                Debug.Log("Le cooldown est terminé, l'ennemi reprend la poursuite.");
            }
        }
    }
}
