using UnityEngine;

public class AttackEnemies : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] public GameObject katana; // Assure-toi que le Katana a un collider valide (BoxCollider, CapsuleCollider, etc.)
    [SerializeField] float attackDuration = 3.20f; // Dur�e de l'attaque en secondes

    private float attackTimer = 0f; // Timer pour suivre le temps d'attaque
    private bool isAttacking = false; // Indique si l'attaque est en cours
    private bool hasHit = false; // Indique si une collision a �t� r�ussie
    private BoxCollider katanaCollider;
    private CapsuleCollider playerCollider;
    private GameObject player;

    void Start()
    {
        // Initialisation des r�f�rences une seule fois
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            katanaCollider = katana.GetComponent<BoxCollider>();
            playerCollider = player.GetComponent<CapsuleCollider>();
        }
    }

    void Update()
    {
        if (isAttacking)
        {
            attackTimer += Time.deltaTime; // Incr�mente le timer � chaque frame pendant l'attaque

            // V�rifie les collisions pendant l'attaque, mais seulement si une collision n'a pas encore eu lieu
            if (!hasHit && katanaCollider != null && playerCollider != null)
            {
                if (katanaCollider.bounds.Intersects(playerCollider.bounds))
                {
                    //Debug.Log("Le katana intersecte le joueur. D�g�ts inflig�s !");
                    player.GetComponent<HealthComponent>()?.TakeDamage(damage);
                    hasHit = true; // Marque la collision comme r�ussie
                }
            }

            // Si le temps de l'attaque est �coul�, arr�te l'attaque
            if (attackTimer >= attackDuration)
            {
                isAttacking = false;
                Debug.Log("L'attaque est termin�e.");
            }
        }
    }

    public void StartAttack()
    {
        if (player != null)
        {
            // R�initialise le timer, l'�tat de l'attaque et le flag de collision
            attackTimer = 0f;
            isAttacking = true;
            hasHit = false; // Autorise une nouvelle collision
            Debug.Log("L'attaque a commenc�.");
        }
        else
        {
            Debug.LogError("Le joueur avec le tag 'Player' est introuvable.");
        }
    }
}
