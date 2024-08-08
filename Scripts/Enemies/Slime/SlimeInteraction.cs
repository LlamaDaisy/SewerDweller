using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeInteraction : MonoBehaviour
{

    [Header("Slime Health Stats")]
    [SerializeField] float health;
    [SerializeField] float slimeMaxHealth;

    [Header("Loot Drops")]
    [SerializeField]
    Transform lootSpawnPoint;

    FloatingHealthBar updateHealthBar;
    PlayerStats playerStats;

    // Start is called before the first frame update
    void Start()
    {
        updateHealthBar = GetComponentInChildren<FloatingHealthBar>();
        health = slimeMaxHealth;
        updateHealthBar.UpdateHealthBar(health, slimeMaxHealth);
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }


    /// <summary>
    /// Handles how much damage slime has taken and updates health bar
    /// </summary>
    /// <param name="damage"></param>
    public void SlimeDamage (float damage)
    {
        health -= damage;
        updateHealthBar.UpdateHealthBar(health, slimeMaxHealth);

        //instantiates loot drop if health is less than 0
        if (health <= 0)
        {
            GetComponent<LootBag>().InstantiateLoot(lootSpawnPoint.position);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// handles damage slime does to player
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerStats.PlayerDamage(1);
        }
    }
}
