using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAnimationControl : MonoBehaviour
{
    public Animator animator;
    private float horizontal;
    //private Rigidbody2D rb;




    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        //rb = GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal != 0f) 
        {
            animator.Play("Walking");
        }

        if (Input.GetButtonDown("Jump"))
        {
            animator.SetTrigger("Jumping");
        }
      
    }
}
