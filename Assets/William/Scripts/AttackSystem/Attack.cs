using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    //
    public GameObject katana1Object;  
    public GameObject katana2Object;  
    private KatanaAttack katana1;  
    private KatanaAttack katana2;  

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
                animator.SetTrigger("LightAttackLeft");
            else if (side == 1)
                animator.SetTrigger("LightAttackRight");

            PerformAttack(lightAttackDamage);
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
            //Debug.LogWarning("Combo d�j� en cours !");
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

    private void PerformAttack(float damage)
    {
        if (katana1 != null && katana2 != null)
        {
            katana1.StartAttack();
            katana2.StartAttack();
        }
        //// Log pour indiquer que l'attaque est lanc�e
        //Debug.Log("Attempting to perform attack with damage: " + damage);

        //// V�rification de la pr�sence d'ennemis dans la port�e
        //Collider[] hitEnemies = Physics.OverlapSphere(curAttackPoint.position, attackRange, enemyLayer);

        //if (hitEnemies.Length == 0)
        //{
        //    Debug.Log("No enemies found within attack range.");
        //}

        //foreach (Collider enemy in hitEnemies)
        //{
        //    // Log pour chaque ennemi d�tect� dans la port�e
        //    Debug.Log("Enemy detected: " + enemy.gameObject.name);

        //    if (enemy.gameObject.CompareTag("Boss"))
        //    {
        //        // Log pour indiquer qu'un boss a �t� trouv�
        //        Debug.Log("Boss detected: " + enemy.gameObject.name);

        //        // V�rifier si le katana touche l'ennemi
        //        if (IsKatanaTouchingEnemy(enemy))
        //        {
        //            // Log lorsque le katana touche le boss
        //            Debug.Log("Katana is touching the boss!");

        //            // Appliquer les d�g�ts au boss
        //            enemy.gameObject.GetComponent<Boss>().TakeDammage(damage);
        //        }
        //        else
        //        {
        //            // Log pour indiquer que le katana ne touche pas le boss
        //            Debug.Log("Katana is NOT touching the boss.");
        //        }
        //    }
        //}
    }

    //private bool IsKatanaTouchingEnemy(Collider enemy)
    //{
    //    // R�cup�rer le BoxCollider du katana (enfant du point d'attaque)
    //    BoxCollider katanaCollider = curAttackPoint.GetComponentInChildren<BoxCollider>();

    //    if (katanaCollider != null)
    //    {
    //        // Log pour afficher les informations du collider du katana
    //        Debug.Log("Katana Collider Bounds: " + katanaCollider.bounds);

    //        // R�cup�rer tous les colliders du boss (tous les os avec des colliders)
    //        Collider[] bossColliders = enemy.gameObject.GetComponentsInChildren<Collider>();

    //        foreach (Collider bossCollider in bossColliders)
    //        {
    //            if (bossCollider != null)
    //            {
    //                // Log pour afficher les informations des colliders du boss
    //                Debug.Log("Boss Collider Bounds: " + bossCollider.bounds);

    //                // V�rifier si le collider du katana entre en collision avec un des colliders du boss
    //                bool isTouching = katanaCollider.bounds.Intersects(bossCollider.bounds);

    //                // Log pour savoir si les colliders se touchent ou non
    //                if (isTouching)
    //                {
    //                    Debug.Log("Katana touching boss's collider: " + bossCollider.gameObject.name);
    //                    return true; // Si on touche un collider du boss, on retourne vrai
    //                }
    //            }
    //        }

    //        // Si aucun collider du boss n'est touch�
    //        Debug.Log("Katana is NOT touching any of the boss's colliders.");
    //        return false;
    //    }
    //    else
    //    {
    //        // Log si le BoxCollider du katana est manquant
    //        Debug.LogWarning("Collider(s) missing: KatanaCollider");
    //        return false;
    //    }
    //}

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

        Vector3 currentScale = kunai.transform.localScale;
        kunai.transform.localScale = new Vector3(currentScale.x * 10, currentScale.y * 10, currentScale.z * 10);

        rb.isKinematic = false; 
        rb.useGravity = false;  

        rb.velocity = transform.forward * kunaiSpeed;

        Destroy(kunai, kunaiLifetime);
    }
}
