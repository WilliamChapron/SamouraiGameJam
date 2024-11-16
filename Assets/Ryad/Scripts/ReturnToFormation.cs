using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ReturnToFormation : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;

    void Start()
    {
        // Assurez-vous d'initialiser l'agent
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("l'Enemie retourne dans sa formation");
            Vector3 direction = (transform.position - player.position).normalized;
            Vector3 destination = transform.position + direction * 10;

            agent.SetDestination(destination);
        }
    }
}