using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] public Transform attackPoint1;
    [SerializeField] public Transform attackPoint2;

    private Transform curAttackPoint;

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

    [Header("Kunai Attack Settings")]
    public GameObject kunaiPrefab; // Préfab du kunai
    public Transform kunaiSpawnPoint; // Point de départ du kunai
    public float kunaiAttackCooldown = 1f; // Temps de recharge entre deux attaques
    public float kunaiSpeed = 15f; // Vitesse de lancement du kunai
    public float kunaiLifetime = 5f; // Durée de vie du kunai après son lancement
    private float lastKunaiAttackTime = 0f; // Temps du dernier lancement de kunai

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
    private bool isKunaiAttacking = false;

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
    public bool IsKunaiAttacking
    {
        get { return isKunaiAttacking; }
        set { isKunaiAttacking = value; }
    }

    private void Start()
    {
        curAttackPoint = attackPoint1;
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            curAttackPoint = attackPoint1;
            AttackLight(0);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            curAttackPoint = attackPoint2;
            AttackLight(1);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            AttackCombo();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            AttackKunai();
        }

    }

    private void AttackLight(int side)
    {
        attackPoint1.gameObject.SetActive(true);
        attackPoint2.gameObject.SetActive(true);
        if (isLightAttacking)
        {
            //Debug.LogWarning("Attaque légère déjà en cours !");
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
            //Debug.LogWarning("Attaque légère en cooldown !");
        }
    }

    private void AttackCombo()
    {
        if (isComboAttacking)
        {
            //Debug.LogWarning("Combo déjà en cours !");
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
            //Debug.LogWarning("Impossible d'enchaîner le combo !");
        }
        else
        {
            //Debug.LogWarning("Le combo est en cooldown !");
        }
    }

    private void AttackKunai()
    {
        if (isKunaiAttacking)
        {
            return;
        }

        if (Time.time >= lastKunaiAttackTime + kunaiAttackCooldown)
        {
            // Desactive Katana
            attackPoint1.gameObject.SetActive(false);
            attackPoint2.gameObject.SetActive(false);
            //
            isKunaiAttacking = true;

            animator.SetTrigger("ThrowKunai");
            LaunchKunai();
            lastKunaiAttackTime = Time.time;
        }
    }

    private IEnumerator EnableComboWindow()
    {
        canCombo = true;
        //Debug.Log("Fenêtre d'enchaînement combo activée !");
        yield return new WaitForSeconds(comboWindow);
        canCombo = false;
        //Debug.Log("Fenêtre d'enchaînement combo fermée.");
    }

    private void PerformAttack(float damage)
    {
        Collider[] hitEnemies = Physics.OverlapSphere(curAttackPoint.position, attackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            if(enemy.gameObject.tag == "Boss") {
                enemy.gameObject.GetComponent<Boss>().TakeDammage(5);
            }
        }

        if (hitEnemies.Length == 0)
        {
        }
    }

    private void LaunchKunai()
    {
        if (kunaiPrefab == null || kunaiSpawnPoint == null)
        {
            Debug.LogWarning("Kunai prefab or spawn point is not assigned.");
            return;
        }

        GameObject kunai = Instantiate(kunaiPrefab, kunaiSpawnPoint.position, kunaiSpawnPoint.rotation);

 
        Rigidbody rb = kunai.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = kunai.AddComponent<Rigidbody>(); 
        }

        rb.isKinematic = false; 
        rb.useGravity = false;  

        rb.velocity = transform.forward * kunaiSpeed;

        Destroy(kunai, kunaiLifetime);
    }
}
