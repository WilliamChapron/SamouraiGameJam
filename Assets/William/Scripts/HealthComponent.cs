using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public Animator animator;

    [Header("Health Settings")]
    public float maxHealth = 100f; 
    private float currentHealth;   

    [Header("Death Settings")]
    public bool isDead = false; 

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
    }

    private void Die()
    {
        if (isDead) return; 
        isDead = true;


        animator.SetBool("isDead", true);

        // Désactiver ou détruire le joueur après la mort
        Destroy(gameObject, 3f); // Détruire le personnage après 3 secondes (ou autre logique de fin)
    }

    //public void Heal(float healAmount)
    //{
    //    if (isDead) return; 

    //    currentHealth += healAmount;
    //    if (currentHealth > maxHealth) currentHealth = maxHealth; 

    //    Debug.Log($"Player healed by {healAmount}. Current health: {currentHealth}");
    //}
}