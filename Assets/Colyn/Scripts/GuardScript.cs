using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GuardScript : EnemyScript
{
    [SerializeField] public GameObject katana;
    [SerializeField] float attackDuration = 1f; // Dur�e de l'attaque en secondes

    private BoxCollider katanaCollider;

    private float attackTimer = 0f; // Timer pour suivre le temps d'attaque
    private bool isAttacking = false; // Indique si l'attaque est en cours
    private bool hasHit = false; // Indique si une collision a �t� r�ussie

    float rotationToY;
    public int knockbackAmount = 0;

    float maxTimeUntilKnockbackReset = 1.5f;
    float timeUntilKnockbackReset = 1.5f;

    public GuardKnockbackState knockbackState;
    public StateManager stateManager;

    public override void Start()
    {
        damage = 5.0f;
        stateManager = GetComponent<StateManager>();

        moveSpeed = 2.0f;

        maxAttackCooldown = 5.0f;

        base.Start();

        katanaCollider = katana.GetComponent<BoxCollider>();

        healthComponent.maxHealth = 50.0f;
    }

    public void StartAttack()
    {
        if (playerObject != null)
        {
            // R�initialise le timer, l'�tat de l'attaque et le flag de collision
            attackTimer = 0f;
            isAttacking = true;
            hasHit = false; // Autorise une nouvelle collision
            Debug.Log("L'attaque a commenc�.");

            PlayAttack();
        }
        else
        {
            Debug.LogError("Le joueur avec le tag 'Player' est introuvable.");
        }
    }

    public override void TakeDamage(int damage)
    {
        katanaCollider = katana.GetComponent<BoxCollider>();

        timeUntilKnockbackReset = maxTimeUntilKnockbackReset;

        knockbackAmount++;

        knockbackState.SetKnockbackTime();
        stateManager.SwitchToNextState(knockbackState);



        base.TakeDamage(damage);
    }

    public override void Update()
    {
        base.Update();

        // Turn to player

        Vector3 lookDirection = playerTransform.position - transform.position;
        lookDirection.y = 0; // Gardez l'axe Y constant pour �viter de pencher vers le haut ou le bas
        transform.rotation = Quaternion.LookRotation(lookDirection);

        timeUntilKnockbackReset -= Time.deltaTime;

        if (knockbackAmount != 0  && timeUntilKnockbackReset <= 0)
        {
            knockbackAmount = 0;
            Debug.Log("Knockback Reset");
        }

        if (isAttacking)
        {
            attackTimer += Time.deltaTime; // Incr�mente le timer � chaque frame pendant l'attaque

            // V�rifie les collisions pendant l'attaque, mais seulement si une collision n'a pas encore eu lieu
            if (!hasHit && katanaCollider != null && playerCollider != null)
            {
                if (katanaCollider.bounds.Intersects(playerCollider.bounds))
                {
                    //Debug.Log("Le katana intersecte le joueur. D�g�ts inflig�s !");
                    playerObject.GetComponent<HealthComponent>()?.TakeDamage(damage);
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

        Vector3 worldDeltaPosition = agent.destination - transform.position;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);

        animator.SetFloat("VelocityX", dx);
        animator.SetFloat("VelocityZ", dy);
    }
}
