using UnityEngine;

public class AttackEnemies : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] public GameObject katana; // Assure-toi que le Katana a un collider valide (BoxCollider, CapsuleCollider, etc.)
    [SerializeField] float attackDuration = 3.20f; // Durée de l'attaque en secondes

    [SerializeField] float Maxcmp = 0.5f;
    public float cmp = 0f;
    private bool CmpFinish = true; // Indique si la période de coup est terminée
    private float attackTimer = 0f; // Timer pour suivre le temps d'attaque
    private bool isAttacking = false; // Indique si l'attaque est en cours
    private bool hasHit = false; // Indique si une collision a été réussie
    private BoxCollider katanaCollider;
    private CapsuleCollider playerCollider;
    private GameObject player;

    void Start()
    {
        // Initialisation des références une seule fois
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            katanaCollider = katana.GetComponent<BoxCollider>();
            playerCollider = player.GetComponent<CapsuleCollider>();
        }
        else
        {
            Debug.LogError("Le joueur avec le tag 'Player' est introuvable.");
        }
    }

    void Update()
    {
        if (isAttacking)
        {
            if (!CmpFinish)
            {
                cmp -= Time.deltaTime; // Décrémente cmp
                if (cmp <= 0f)
                {
                    hasHit = false;
                    CmpFinish = true; // Permet de frapper à ce moment-là
                }
            }

            attackTimer += Time.deltaTime; // Incrémente le timer à chaque frame pendant l'attaque

            // Ne détecte la collision que pendant une certaine période de l'attaque (par exemple, de 0.3 à 2.5 secondes)
            if (attackTimer >= 0.3f && attackTimer <= 2.5f)
            {
                DealDamage(); // Inflige les dégâts au joueur
            }

            // Si le temps de l'attaque est écoulé, arrête l'attaque
            if (attackTimer >= attackDuration)
            {
                isAttacking = false;
                Debug.Log("L'attaque est terminée.");
            }
        }
    }

    private void DealDamage()
    {
        // Si le katana entre en collision avec le joueur, inflige des dégâts
        if (katanaCollider != null && playerCollider != null && !hasHit)
        {
            if (katanaCollider.bounds.Intersects(playerCollider.bounds))
            {
                // Inflige des dégâts au joueur
                player.GetComponent<HealthComponent>()?.TakeDamage(damage);
                Debug.Log("Dégâts infligés au joueur !");
                hasHit = true; // Empêche de frapper plusieurs fois durant l'attaque
            }
        }
    }

    public void StartAttack()
    {
        if (player != null)
        {
            // Réinitialise le timer, l'état de l'attaque et le flag de collision
            cmp = Maxcmp;
            attackTimer = 0f;
            isAttacking = true;
            CmpFinish = false; // Le coup est prêt à frapper
            hasHit = false; // Réinitialise l'état de la collision
            Debug.Log("L'attaque a commencé.");
        }
        else
        {
            Debug.LogError("Le joueur avec le tag 'Player' est introuvable.");
        }
    }
}