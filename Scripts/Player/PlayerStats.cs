using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    Animator PlayerAnimator;
    [SerializeField]
    public float health;
    [SerializeField]
    public float maxHealth = 10;
    public Slider healthSlider;

    [Header ("Player Score")]

    public float score;
    [SerializeField]
    public TMP_Text playerScore;

    FloatingHealthBar updateHealthBar;

    private void Start()
    {
        health = maxHealth;
        updateHealthBar = GetComponentInChildren<FloatingHealthBar>();
        updateHealthBar.UpdateHealthBar(health, maxHealth);
        PlayerAnimator = GetComponentInChildren<Animator>();

    }

    //update score text in game - without this score will increase but wont be show in the text.
    void Update()
    {
        playerScore.text = "" + score;
        updateHealthBar.UpdateHealthBar(health, maxHealth);
    }

    /// <summary>
    /// Handles player health and damage
    /// </summary>
    /// <param name="damage"></param>
    public void PlayerDamage (float damage)
    {
        health -= damage;
        updateHealthBar.UpdateHealthBar(health, maxHealth);
        PlayerAnimator.SetTrigger("takeDamage");

        if (health <= 0) 
        {
            Die();
        }
    }

    /// <summary>
    /// Handles health restore
    /// </summary>
    /// <param name="amount"></param>
    public void RestoreHealth(float amount)
    {
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        updateHealthBar.UpdateHealthBar(health, maxHealth);
    }

    /// <summary>
    /// Handles player death 
    /// </summary>
    private void Die ()
    {
        Debug.Log("Player Dead");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
