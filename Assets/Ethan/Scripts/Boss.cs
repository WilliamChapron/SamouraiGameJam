using System;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour {

    [Header("Setup")]
    [SerializeField] new string name = "BOSS";

    [Range(0, 2)]
    [SerializeField] int state = 0;

    [Serializable] struct Stats {
        [Range(0, 100)] public float health;
        [Range(0, 100)] public float attack;
    }
    [SerializeField] Stats stats;

    [Serializable]
    struct Detection {
        [Range(0, 100)] public float range;
        [Range (0, 100)] public float minDistance;
        public LayerMask mask;
        public string tag;
    }
    [SerializeField] Detection detection;

    [Serializable] struct Movements {
        [Range(0, 100)] public float range;
    }
    [SerializeField] Movements movements;

    // Privates
    NavMeshAgent agent;
    Vector3 original;
    Vector3 target;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        original = transform.position;
    }

    void Update() {
        Collider[] colliders = Physics.OverlapSphere(original, detection.range, detection.mask);
        switch (state) {
            case 0:
                foreach (Collider collider in colliders) {
                    if (collider.tag == detection.tag) agent.SetDestination(collider.gameObject.transform.position);
                }
                break;
            case 1:
                if (agent.remainingDistance < detection.minDistance) {
                    foreach (Collider collider in colliders) {
                        if (collider.tag == detection.tag) agent.SetDestination(collider.gameObject.transform.position);
                    }
                }
                break;
            case 2:
                agent.SetDestination(original);
                break;
            default:
                break;
        }

    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, movements.range);

        if(!Application.isPlaying) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detection.range);
        } else {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(original, detection.range);
        }
    }
}