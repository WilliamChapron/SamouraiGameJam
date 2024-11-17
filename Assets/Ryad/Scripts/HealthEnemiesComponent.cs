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

    [Header("Death Settings")]
    public bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;

        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        lifebar.value = currentHealth / maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= damageAmount;


        Debug.Log($"Player took {damageAmount} damage. Current health: {currentHealth}");

        animator.SetTrigger("TakeHit");


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