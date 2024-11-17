using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Boss : MonoBehaviour {

    [Header("Setup")]
    [SerializeField] new string name = "BOSS";

    [SerializeField] HurtPlayer pl_y;

    [SerializeField] Slider slider;

    [Range(0, 3)]
    [SerializeField] int state = 0;
    
    float health;

    [Serializable] struct Stats {
        [Range(0, 4000)] public float health;
        [HideInInspector] public float time;
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

    [Serializable] struct Standard {
        public float dammage;
        public float duration;
        public float interval;

        [HideInInspector] public float time;
    }
    [Serializable] struct Rain {
        public GameObject projectile;
        public float duration;
        public float interval;
        [HideInInspector] public float time;
    }
    [Serializable] struct Attacks {
        public Standard standard;
        public Rain rain;
    }
    [SerializeField] Attacks attacks;


    // Privates
    Animator animator;
    NavMeshAgent agent;

    Vector3 original;
    float originalSpeed;

    void Start() {
        health = stats.health;
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        original = transform.position;
        originalSpeed = agent.speed;
    }

    float time = 0;

    void Update() {
        slider.value = health / stats.health;

        Collider[] colliders = Physics.OverlapSphere(original, detection.range, detection.mask);

        animator.SetBool("Walk", agent.velocity.magnitude != 0 && agent.speed == originalSpeed);
        animator.SetBool("Run", agent.velocity.magnitude != 0 && agent.speed == originalSpeed * 3);

        if(stats.time < 30) { stats.time += Time.deltaTime; }
        else { state = UnityEngine.Random.Range(0, 3); stats.time = 0; }
        
        switch (state) {
            case 0:
                agent.speed = originalSpeed;

                foreach (Collider collider in colliders) {
                    if (collider.tag == detection.tag) agent.SetDestination(collider.gameObject.transform.position);
                }

                if(agent.isStopped) {
                    if (attacks.standard.time < attacks.standard.duration) { attacks.standard.time += Time.deltaTime; }
                    else { attacks.standard.time = 0; agent.isStopped = false; }
                }

                if (agent.remainingDistance < detection.minDistance && !agent.isStopped) {
                    agent.isStopped = true;

                    bool foundPlayer = false;

                    foreach (Collider collider in colliders)
                    {
                        if (collider.tag == detection.tag) { foundPlayer = true; }
                    }

                    if(foundPlayer) { animator.SetTrigger("Attack2"); pl_y.hurt = true; }
                    else { animator.SetTrigger("Idle"); pl_y.hurt = false; }

                }
                break;
            case 1:

                NavMeshHit hit;
                agent.speed = originalSpeed * 3;
                if (agent.remainingDistance < detection.minDistance || !NavMesh.SamplePosition(agent.destination, out hit, detection.range, NavMesh.AllAreas)) {
                    foreach (Collider collider in colliders) {
                        if (collider.tag == detection.tag) agent.SetDestination(collider.gameObject.transform.position);
                    }
                }
                break;
            case 2:
                agent.SetDestination(original);
                if (agent.remainingDistance <= 0) { animator.SetTrigger("Meteor"); state = 3; }
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

    public void Die() {
        Destroy(gameObject);
    }

    public void TakeDammage(float amount)
    {
        if (health > 0)
        {
            health -= amount;
            Debug.Log("Boss Health: " + health); 
        }
        else
        {
            Die();
        }
    }

    private void OnDrawGizmos() {
        if(!Application.isPlaying) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detection.range);
        } else {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(original, detection.range);
        }
    }
}