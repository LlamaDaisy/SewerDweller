using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformLRMovement : MonoBehaviour
{
    [SerializeField]
    GameObject[] wayPoints;
    [SerializeField]
    float speed;
    [SerializeField]
    float prox;

    Rigidbody2D rb;

    int currentIndex;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentIndex = 0;
    }

    private void Update()
    {
        MoveToCurrentPoint();
        UpdatePatrol();
    }

    /// <summary>
    /// Moves platform to current waypoint
    /// </summary>
    void MoveToCurrentPoint()
    {
        Transform targetPoint = wayPoints[currentIndex].transform;

        Vector2 direction = (targetPoint.position - transform.position).normalized;

        rb.velocity = direction * speed;
    }

    /// <summary>
    /// Update the waypoint index when the current one is reached
    /// </summary>
    void UpdatePatrol()
    {
        //Move to the next waypoint, looping back to the first waypoint if at the end.
        if (isCloseToCurrentPoint())
        {
            currentIndex = (currentIndex + 1) % wayPoints.Length;
        }
    }
    /// <summary>
    /// Checks if the platform is close enough to the current waypoint.
    /// </summary>
    /// <returns></returns>
    bool isCloseToCurrentPoint()
    {
        Transform targetPoint = wayPoints[currentIndex].transform;
        return Vector2.Distance(transform.position, targetPoint.position) < prox;

    }
}

