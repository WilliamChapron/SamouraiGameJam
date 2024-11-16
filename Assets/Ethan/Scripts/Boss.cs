using System;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour {

    [Header("Setup")]
    [SerializeField] new string name = "BOSS";

    [Range(0, 3)]
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

    [Serializable] struct Standard {
        public float dammage;
    }
    [Serializable] struct Rain {
        public GameObject projectile;
        public float duration;
        public float interval;
        [HideInInspector] public float time;
    }
    [Serializable] struct Attacks {
        public Rain rain;
    }
    [SerializeField] Attacks attacks;


    // Privates
    Animator animator;
    NavMeshAgent agent;
    Vector3 original;
    Vector3 target;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        original = transform.position;
    }
    float time = 0;
    bool attack = false;
    void Update() {
        Collider[] colliders = Physics.OverlapSphere(original, detection.range, detection.mask);

        animator.SetBool("Walk", agent.velocity.magnitude != 0);
        
        switch (state) {
            case 0:
                foreach (Collider collider in colliders) {
                    if (collider.tag == detection.tag) agent.SetDestination(collider.gameObject.transform.position);
                }

                if (agent.remainingDistance > detection.minDistance) attack = false;

                if (agent.remainingDistance < detection.minDistance && !attack) {
                    animator.SetTrigger("Attack1");
                    attack = true;
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
            case 3:
                if (time < attacks.rain.duration) { time += Time.deltaTime; }
                else { time = 0; state = 0; break; }
                
                if(attacks.rain.time < attacks.rain.interval) { attacks.rain.time += Time.deltaTime; }
                else {
                    GameObject projectile = Instantiate(attacks.rain.projectile) as GameObject;
                    projectile.transform.position = transform.position + new Vector3(UnityEngine.Random.Range(-detection.range, detection.range), 50, UnityEngine.Random.Range(-detection.range, detection.range));
                    attacks.rain.time = 0;
                }
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