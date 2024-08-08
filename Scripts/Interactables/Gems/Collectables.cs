using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public Loot lootData;
    private PlayerStats playerStats;

    private void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    /// <summary>
    /// Handles the collection of loot when the player enters the trigger collider.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (lootData != null)
            {
                switch (lootData.lootName)
                {
                    case "Gem":
                        Debug.Log("Gem Collected");
                        playerStats.score += 1;
                        break;

                    case "Health":
                        Debug.Log("Health Collected");
                        playerStats.RestoreHealth(1);
                        break;

                        // Add more cases for other loot types as needed
                }
            }

            StartCoroutine(DestoryAfterDelay(0.3f));
        }
    }

    /// <summary>
    /// Coroutine that waits for a specified delay before destroying the game object.
    /// </summary>
    /// <param name="delay">The delay in seconds before the game object is destroyed.</param>
    /// <returns>IEnumerator for the coroutine.</returns>
    private IEnumerator DestoryAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
