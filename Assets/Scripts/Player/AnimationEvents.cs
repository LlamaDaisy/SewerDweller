using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles animation events
/// </summary>
public class AnimationEvents : MonoBehaviour
{
    public GameObject player;

    /// <summary>
    /// End attack animation
    /// </summary>
    public void EndAttack()
    {
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();

        if (playerMovement != null ) 
        {
            playerMovement.EndPlayerAttack();
        }
    }

    /// <summary>
    /// Start attack animation
    /// </summary>
    public void StartAttack() 
    { 
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();

        if (playerMovement !=null)
        {
            playerMovement.Attack();
        }

    }

}
