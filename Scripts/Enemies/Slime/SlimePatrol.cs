using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlimePatrol : MonoBehaviour
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

    void MoveToCurrentPoint ()
    {
        Transform targetPoint  = wayPoints[currentIndex].transform;

        Vector2 direction = (targetPoint.position - transform.position).normalized;

        rb.velocity = direction * speed;
    }

    void UpdatePatrol () 
    { 
        if (isCloseToCurrentPoint())
        {
            currentIndex = (currentIndex + 1) % wayPoints.Length;
        }
    }

    bool isCloseToCurrentPoint () 
    {
        Transform targetPoint = wayPoints[currentIndex].transform;
        return Vector2.Distance(transform.position, targetPoint.position) < prox;
    
    }
}
