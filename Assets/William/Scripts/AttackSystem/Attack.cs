using System.Collections;
using UnityEngine;

public class AttackSystemDebug : MonoBehaviour
{
    [Header("General Settings")]
    public Transform attackPoint; // Point d'origine des attaques
    public float attackRange = 2f; // Port�e des attaques
    public LayerMask enemyLayer; // Couches � attaquer

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
    public GameObject debugTrailPrefab; // Effet de tra�n�e pour visualiser l'attaque
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
        if (Input.GetKeyDown(KeyCode.Mouse0)) // Clic gauche pour attaque l�g�re
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
            Debug.Log("Lancement de l'attaque l�g�re !");
            //animator.SetTrigger("AttackLight");
            PerformAttack(lightAttackDamage);
            //PlayAttackEffects("light");
            StartCoroutine(EnableComboWindow());
            lastLightAttackTime = Time.time;

            //// G�n�rer une tra�n�e pour l'attaque l�g�re
            //CreateDebugTrail();
        }
        else
        {
            Debug.LogWarning("Attaque l�g�re en cooldown !");
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

            // G�n�rer une tra�n�e pour le combo
            CreateDebugTrail(true);
        }
        else if (!canCombo)
        {
            Debug.LogWarning("Impossible d'encha�ner le combo !");
        }
        else
        {
            Debug.LogWarning("Le combo est en cooldown !");
        }
    }

    private IEnumerator EnableComboWindow()
    {
        canCombo = true;
        Debug.Log("Fen�tre d'encha�nement combo activ�e !");
        yield return new WaitForSeconds(comboWindow);
        canCombo = false;
        Debug.Log("Fen�tre d'encha�nement combo ferm�e.");
    }

    private void PerformAttack(float damage)
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider enemy in hitEnemies)
        {
            //if (enemy.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
            //{
            //    enemyHealth.TakeDamage(damage);
            //    Debug.Log($"Attaque r�ussie sur {enemy.name} ! D�g�ts inflig�s : {damage}");
            //}
        }

        if (hitEnemies.Length == 0)
        {
            Debug.Log("Aucun ennemi touch�.");
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
            Destroy(trail, 1f); // D�truire la tra�n�e apr�s 1 seconde
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        // Visualiser la port�e d'attaque
        Gizmos.color = debugAttackRangeColor;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);

        // Indiquer si le combo est activable
        if (Application.isPlaying && canCombo)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange * 1.2f); // Cercle l�g�rement plus large
        }
    }
}
