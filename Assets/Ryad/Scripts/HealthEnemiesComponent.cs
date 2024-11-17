using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthEnemiesComponent : MonoBehaviour
{

    private Animator animator;

    [Header("Health Settings")]
    public float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    [SerializeField] Slider lifebar;
    private Camera mainCamera;

    [Header("Death Settings")]
    public bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponentInChildren<Animator>();

        if (lifebar != null)
        {
            lifebar.maxValue = maxHealth;
            lifebar.value = currentHealth;
        }

        GameObject cameraObject = GameObject.FindWithTag("MainCamera");
        if (cameraObject != null)
        {
            mainCamera = cameraObject.GetComponent<Camera>();  // Récupère le composant Camera
        }
        else
        {
            Debug.LogError("MainCamera not found. Please ensure an object with the tag 'MainCamera' exists in the scene.");
        }
    }

    private void Update()
    {
        if (lifebar != null)
        {
            lifebar.value = currentHealth;  // Met à jour la barre de vie
        }

        // Orienter la barre de vie pour qu'elle soit toujours visible et orientée vers la caméra
        if (lifebar != null && mainCamera != null)
        {
            lifebar.transform.LookAt(lifebar.transform.position + mainCamera.transform.rotation * Vector3.forward,
                                    mainCamera.transform.rotation * Vector3.up);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= damageAmount;

        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0); 
        if (!currentState.IsName("ReactionHit"))
        {
            animator.SetTrigger("TakeHit");
        }


        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        animator.SetTrigger("Die");

        Destroy(gameObject, 3f); // Détruire le personnage après 3 secondes (ou autre logique de fin)
    }
}