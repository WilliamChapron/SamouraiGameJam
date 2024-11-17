using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    //
    public GameObject katana1Object;  
    public GameObject katana2Object;  
    public KatanaAttack katana1;  
    public KatanaAttack katana2;  

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
    public GameObject kunaiPrefab; 
    public Transform kunaiSpawnPoint; 
    public float kunaiAttackCooldown = 1f; 
    public float kunaiSpeed = 15f; 
    public float kunaiLifetime = 5f; 
    private float lastKunaiAttackTime = 0f; 

    [Header("Effects & Debugging")]

    private Animator animator;
    private HealthComponent healthComponent;

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
        healthComponent = GetComponent<HealthComponent>();

        katana1 = katana1Object.GetComponent<KatanaAttack>();
        katana2 = katana2Object.GetComponent<KatanaAttack>();
    }

    private void Update()
    {
        if (healthComponent.isDead) return; 

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
            //Debug.LogWarning("Attaque l�g�re d�j� en cours !");
            return;
        }

        if (Time.time >= lastLightAttackTime + lightAttackCooldown)
        {
            isLightAttacking = true;

            if (side == 0)
            {
                animator.SetTrigger("LightAttackLeft");
                katana1.StartAttack();
                katana1.isAttackON = true;
            }
                

            else if (side == 1)
            {
                animator.SetTrigger("LightAttackRight");
                katana2.StartAttack();
                katana2.isAttackON = true;
            }
                


            StartCoroutine(EnableComboWindow());
            lastLightAttackTime = Time.time;
        }
        else
        {
            //Debug.LogWarning("Attaque l�g�re en cooldown !");
        }
    }

    private void AttackCombo()
    {
        if (isComboAttacking)
        {
            //katana1.isAttackON = true;
            //katana2.isAttackON = true;
            //Debug.LogWarning("Combo d�j� en cours !");
            return;
        }

        if (canCombo && Time.time >= lastComboTime + comboCooldown)
        {
            isComboAttacking = true;

            animator.SetTrigger("ComboAttack");


            //Debug.Log("Active 2 Katana");
            katana1.StartAttack();
            katana2.StartAttack();

            katana1.isAttackON = true;
            katana2.isAttackON = true;

            lastComboTime = Time.time;
            canCombo = false;
        }
        else if (!canCombo)
        {
            //Debug.LogWarning("Impossible d'encha�ner le combo !");
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
        //Debug.Log("Fen�tre d'encha�nement combo activ�e !");
        yield return new WaitForSeconds(comboWindow);
        canCombo = false;
        //Debug.Log("Fen�tre d'encha�nement combo ferm�e.");
    }

    private void LaunchKunai()
    {
        if (kunaiPrefab == null || kunaiSpawnPoint == null)
        {
            Debug.LogWarning("Kunai prefab or spawn point is not assigned.");
            return;
        }

        // Cr�er l'objet kunai � la position du spawn avec une rotation vierge (pour ajuster la direction)
        Quaternion rotationWithX90 = Quaternion.Euler(0f, -90f, 0f) * transform.rotation;
        GameObject kunai = Instantiate(kunaiPrefab, kunaiSpawnPoint.position, rotationWithX90);

        // R�cup�rer ou ajouter un Rigidbody au kunai
        Rigidbody rb = kunai.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = kunai.AddComponent<Rigidbody>();
        }

        // Assurez-vous que le Rigidbody n'est pas cin�matique et utilise la gravit�
        rb.isKinematic = false;
        rb.useGravity = false;


        Vector3 directionToThrow = transform.forward;  
        rb.velocity = directionToThrow * kunaiSpeed;

        // Vous pouvez toujours appliquer une rotation si n�cessaire (ajuster la vitesse de spin)
        // rb.angularVelocity = new Vector3(10f, 0, 0); // Optionnel pour faire tourner le kunai

        // Agrandir le kunai pour un meilleur effet visuel
        Vector3 currentScale = kunai.transform.localScale;
        kunai.transform.localScale = new Vector3(currentScale.x * 4, currentScale.y * 4, currentScale.z * 4);

        // D�tuire le kunai apr�s un certain temps
        Destroy(kunai, kunaiLifetime);
    }
}
