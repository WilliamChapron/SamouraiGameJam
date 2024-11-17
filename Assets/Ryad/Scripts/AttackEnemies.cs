using UnityEngine;

public class AttackEnemies : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] public GameObject katana; // Assure-toi que le Katana a un collider valide (BoxCollider, CapsuleCollider, etc.)
    [SerializeField] float attackDuration = 3.20f; // Dur�e de l'attaque en secondes

    [SerializeField] float Maxcmp = 0.5f;
    public float cmp = 0f;
    private bool CmpFinish = true; // Indique si la p�riode de coup est termin�e
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
                cmp -= Time.deltaTime; // D�cr�mente cmp
                if (cmp <= 0f)
                {
                    hasHit = false;
                    CmpFinish = true; // Permet de frapper � ce moment-l�
                }
            }

            attackTimer += Time.deltaTime; // Incr�mente le timer � chaque frame pendant l'attaque

            // Ne d�tecte la collision que pendant une certaine p�riode de l'attaque (par exemple, de 0.3 � 2.5 secondes)
            if (attackTimer >= 0.3f && attackTimer <= 2.5f)
            {
                DealDamage(); // Inflige les d�g�ts au joueur
            }

            // Si le temps de l'attaque est �coul�, arr�te l'attaque
            if (attackTimer >= attackDuration)
            {
                isAttacking = false;
                Debug.Log("L'attaque est termin�e.");
            }
        }
    }

    private void DealDamage()
    {
        // Si le katana entre en collision avec le joueur, inflige des d�g�ts
        if (katanaCollider != null && playerCollider != null && !hasHit)
        {
            if (katanaCollider.bounds.Intersects(playerCollider.bounds))
            {
                // Inflige des d�g�ts au joueur
                player.GetComponent<HealthComponent>()?.TakeDamage(damage);
                Debug.Log("D�g�ts inflig�s au joueur !");
                hasHit = true; // Emp�che de frapper plusieurs fois durant l'attaque
            }
        }
    }

    public void StartAttack()
    {
        if (player != null)
        {
            // R�initialise le timer, l'�tat de l'attaque et le flag de collision
            cmp = Maxcmp;
            attackTimer = 0f;
            isAttacking = true;
            CmpFinish = false; // Le coup est pr�t � frapper
            hasHit = false; // R�initialise l'�tat de la collision
            Debug.Log("L'attaque a commenc�.");
        }
        else
        {
            Debug.LogError("Le joueur avec le tag 'Player' est introuvable.");
        }
    }
}