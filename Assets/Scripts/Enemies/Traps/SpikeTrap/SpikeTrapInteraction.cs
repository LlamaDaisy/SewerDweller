using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapInteraction : MonoBehaviour
{
    PlayerStats playerStats;

    private void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    /// <summary>
    /// On collision with player deal 2 damage
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
