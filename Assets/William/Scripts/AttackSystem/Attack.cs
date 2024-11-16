using System.Collections;
using UnityEngine;

public class AttackSystemDebug : MonoBehaviour
{
    [Header("General Settings")]
    public Transform attackPoint; // Point d'origine des attaques
    public float attackRange = 2f; // Portée des attaques
    public LayerMask enemyLayer; // Couches à attaquer

    [Header("Light Attack Settings")]
    public float lightAttackDamage = 10f;
    public float lightAttackCooldown = 0.5f;
    private float lastLightAttackTime = 0f;

    [Header("Combo Attack Settings")]
    public float comboDamage = 25f;
    public float comboCooldown = 2f;
    public float comboWindow = 1f;
    private float lastComboTime = 0f;
    private bool canCombo = false;

    [Header("Effects & Debugging")]
    public GameObject lightAttackEffect;
    public GameObject comboAttackEffect;
    public GameObject debugTrailPrefab; // Effet de traînée pour visualiser l'attaque
    public Color debugAttackRangeColor = Color.red;
    public AudioClip lightAttackSound;
    public AudioClip comboAttackSound;

    private AudioSource audioSource;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) // Clic gauche pour attaque légère
        {
            AttackLight();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1)) // Clic droit pour attaque combo
        {
            AttackCombo();
        }
    }

    private void AttackLight()
    {
        if (Time.time >= lastLightAttackTime + lightAttackCooldown)
        {
            Debug.Log("Lancement de l'attaque légère !");
            //animator.SetTrigger("AttackLight");
            PerformAttack(lightAttackDamage);
            //PlayAttackEffects("light");
            StartCoroutine(EnableComboWindow());
            lastLightAttackTime = Time.time;

            //// Générer une traînée pour l'attaque légère
            //CreateDebugTrail();
        }
        else
        {
            Debug.LogWarning("Attaque légère en cooldown !");
        }
    }

    private void AttackCombo()
    {
        if (canCombo && Time.time >= lastComboTime + comboCooldown)
        {
            Debug.Log("Lancement de l'attaque combo !");
            //animator.SetTrigger("AttackCombo");
            PerformAttack(comboDamage);
            //PlayAttackEffects("combo");
            lastComboTime = Time.time;
            canCombo = false;

            // Générer une traînée pour le combo
            CreateDebugTrail(true);
        }
        else if (!canCombo)
        {
            Debug.LogWarning("Impossible d'enchaîner le combo !");
        }
        else
        {
            Debug.LogWarning("Le combo est en cooldown !");
        }
    }

    private IEnumerator EnableComboWindow()
    {
        canCombo = true;
        Debug.Log("Fenêtre d'enchaînement combo activée !");
        yield return new WaitForSeconds(comboWindow);
        canCombo = false;
        Debug.Log("Fenêtre d'enchaînement combo fermée.");
    }

    private void PerformAttack(float damage)
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider enemy in hitEnemies)
        {
            //if (enemy.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
            //{
            //    enemyHealth.TakeDamage(damage);
            //    Debug.Log($"Attaque réussie sur {enemy.name} ! Dégâts infligés : {damage}");
            //}
        }

        if (hitEnemies.Length == 0)
        {
            Debug.Log("Aucun ennemi touché.");
        }
    }

    private void PlayAttackEffects(string attackType)
    {
        if (attackType == "light")
        {
            audioSource.PlayOneShot(lightAttackSound);
            if (lightAttackEffect != null)
                Instantiate(lightAttackEffect, attackPoint.position, Quaternion.identity);
        }
        else if (attackType == "combo")
        {
            audioSource.PlayOneShot(comboAttackSound);
            if (comboAttackEffect != null)
                Instantiate(comboAttackEffect, attackPoint.position, Quaternion.identity);
        }
    }

    private void CreateDebugTrail(bool isCombo = false)
    {
        if (debugTrailPrefab != null)
        {
            var trail = Instantiate(debugTrailPrefab, attackPoint.position, Quaternion.identity);
            var trailColor = isCombo ? Color.blue : Color.yellow;
            trail.GetComponent<Renderer>().material.color = trailColor;
            Destroy(trail, 1f); // Détruire la traînée après 1 seconde
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        // Visualiser la portée d'attaque
        Gizmos.color = debugAttackRangeColor;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);

        // Indiquer si le combo est activable
        if (Application.isPlaying && canCombo)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange * 1.2f); // Cercle légèrement plus large
        }
    }
}
