using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{

    [SerializeField] GameObject deathScreen;

    private Animator animator;

    [Header("Health Settings")]
    public float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    [SerializeField] Slider lifebar;
    [SerializeField] Volume damageVolume;

    [Header("Death Settings")]
    public bool isDead = false;

    int time = 0, timer = 8;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        lifebar.value = currentHealth / maxHealth;
        DecreaseDamageVolume();
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= damageAmount;

        ActivateDamageVolume();

        Debug.Log($"Player took {damageAmount} damage. Current health: {currentHealth}");

        animator.SetTrigger("TakeHit");


        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void ActivateDamageVolume()
    {
        damageVolume.weight = (0.6f - (currentHealth * 0.5f / maxHealth));
    }

    private void DecreaseDamageVolume()
    {
        damageVolume.weight = Mathf.Lerp(damageVolume.weight, 0, 0.03f);
    }

    IEnumerator WaitForMyBite()
    {
        yield return new WaitForSeconds(4);
        deathScreen.SetActive(true);
        Destroy(gameObject, 3f);
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        animator.SetTrigger("Die");

        StartCoroutine(WaitForMyBite());

    }

    //public void Heal(float healAmount)
    //{
    //    if (isDead) return; 

    //    currentHealth += healAmount;
    //    if (currentHealth > maxHealth) currentHealth = maxHealth; 

    //    Debug.Log($"Player healed by {healAmount}. Current health: {currentHealth}");
    //}
}