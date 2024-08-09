using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatInteraction : MonoBehaviour
{
    [SerializeField]
    float ratMaxHealth;
    [SerializeField]
    public float health;

    [Header("Loot Drops")]
    [SerializeField]
    Transform lootSpawnPoint;

    private PlayerStats playerStats;
    FloatingHealthBar updateHealthBar;

    bool isDead = false;    

    // Start is called before the first frame update
    void Start()
    {
        updateHealthBar = GetComponentInChildren<FloatingHealthBar>();
        health = ratMaxHealth;
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        updateHealthBar.UpdateHealthBar(health, ratMaxHealth);
    }

    /// <summary>
    /// handles damage taken and updates health bar
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage (float damage)
    {
        if (isDead) return;
        if (updateHealthBar != null)
        {
            updateHealthBar.UpdateHealthBar(health, ratMaxHealth);
        }

        health -= damage;
        updateHealthBar.UpdateHealthBar(health, ratMaxHealth);
        

        //handles loot drop 
        if (health <= 0)
        {
            isDead = true;
            GetComponent<LootBag>().InstantiateLoot(lootSpawnPoint.position);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// deals damage to player
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerStats.PlayerDamage(2);
        }
    }
}
