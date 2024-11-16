using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("General Settings")]
    public Transform attackPoint; 
    public float attackRange = 2f; 
    public LayerMask enemyLayer; 

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
    public GameObject debugTrailPrefab; 
    public Color debugAttackRangeColor = Color.red;
    public AudioClip lightAttackSound;
    public AudioClip comboAttackSound;

    private AudioSource audioSource;
    private Animator animator;

    //State
    private bool isLightAttacking = false;
    private bool isComboAttacking = false;

    public bool IsLightAttacking
    {
        get { return isLightAttacking; }
        set { isLightAttacking = value; }
    }
    public bool IsComboAttacking
    {
        get { return isComboAttacking; }
        set { isComboAttacking = value; }
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            AttackLight(0);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            AttackLight(1);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            AttackCombo();
        }

    }

    private void AttackLight(int side)
    {
        if (isLightAttacking)
        {
            Debug.LogWarning("Attaque légère déjà en cours !");
            return;
        }

        if (Time.time >= lastLightAttackTime + lightAttackCooldown)
        {
            isLightAttacking = true;

            if (side == 0)
                animator.SetTrigger("LightAttackLeft");
            else if (side == 1)
                animator.SetTrigger("LightAttackRight");

            PerformAttack(lightAttackDamage);
            StartCoroutine(EnableComboWindow());
            lastLightAttackTime = Time.time;
        }
        else
        {
            Debug.LogWarning("Attaque légère en cooldown !");
        }
    }

    private void AttackCombo()
    {
        if (isComboAttacking)
        {
            Debug.LogWarning("Combo déjà en cours !");
            return;
        }

        if (canCombo && Time.time >= lastComboTime + comboCooldown)
        {
            isComboAttacking = true;

            animator.SetTrigger("ComboAttack");
            PerformAttack(comboDamage);

            lastComboTime = Time.time;
            canCombo = false;
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
            Debug.Log($"Enemy hit: {enemy.name}");
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

    //private void CreateDebugTrail(bool isCombo = false)
    //{
    //    if (debugTrailPrefab != null)
    //    {
    //        var trail = Instantiate(debugTrailPrefab, attackPoint.position, Quaternion.identity);
    //        var trailColor = isCombo ? Color.blue : Color.yellow;
    //        trail.GetComponent<Renderer>().material.color = trailColor;
    //        Destroy(trail, 1f); // Détruire la traînée après 1 seconde
    //    }
    //}

    //private void OnDrawGizmosSelected()
    //{
    //    if (attackPoint == null) return;

    //    // Visualiser la portée d'attaque
    //    Gizmos.color = debugAttackRangeColor;
    //    Gizmos.DrawWireSphere(attackPoint.position, attackRange);

    //    // Indiquer si le combo est activable
    //    if (Application.isPlaying && canCombo)
    //    {
    //        Gizmos.color = Color.green;
    //        Gizmos.DrawWireSphere(attackPoint.position, attackRange * 1.2f); // Cercle légèrement plus large
    //    }
    //}
}
