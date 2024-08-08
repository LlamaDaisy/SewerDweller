using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemInteractable : MonoBehaviour
{
    PlayerStats playerStats;
    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    /// <summary>
    /// Add +1 to health on collision
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerStats.score += 1;
            Destroy(gameObject);
        }
    }
}
