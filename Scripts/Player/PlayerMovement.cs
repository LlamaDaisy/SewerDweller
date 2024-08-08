using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator PlayerAnimator; 

    [Header("Movement Controls")]
    public float speed;
    public float jumpForce;
    private float horizontal;
    public Rigidbody2D rb;
    public ParticleSystem smokeFx;


    [Header ("Ground Check")]
    [SerializeField]
    Transform groundCheck;
    [SerializeField]
    LayerMask groundLayer;
    [SerializeField]
    LayerMask platformLayer;
    [SerializeField]
    LayerMask movingPlatformLayer;

    bool facingRight = true;

    public bool doubleJump;
    float jumpAmount;


    [Header ("Gravity")]
    [SerializeField]
    float baseGravity = 2f;
    [SerializeField]
    float maxFallSpeed = 18f;
    [SerializeField]
    float fallSpeedMultipler = 2f;

    [Header("Wall Movement")]
    [SerializeField]
    float wallSlideSpeed;
    public bool isWallSliding;

    bool isWallJumping;
    float wallJumpDirection;
    float wallJumpTime = 0.5f;
    float wallJumpTimer;
    [SerializeField]
    float wallJumpForce;
    public Vector2 wallJumpPower;

    [Header("MovingPlatform")]
    Rigidbody2D platformRb;
    bool isOnMovingPlatform;
    private Transform currentMovingPlatform;

    [Header("Player Attack")]
    [SerializeField] GameObject attackPoint;
    [SerializeField] float attackRadius;
    [SerializeField] LayerMask enemyLayer;

    [Header ("Wall Check")]
    [SerializeField] Transform wallCheck;
    [SerializeField] LayerMask wallLayer;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponentInChildren<Animator>();
        doubleJump = true; 
    }

    // Update is called once per frame
    void Update()
    {
        //When player touching ground set jumpAmount variable to 0
        if (OnGround())
        {
            jumpAmount = 0;
            PlayerAnimator.SetBool("isGrounded", true);
            PlayerAnimator.SetBool("isFalling", false);
        }

        else
        {
            PlayerAnimator.SetBool("isGrounded", false);
            
            if (rb.velocity.y < 0)
            {
                PlayerAnimator.SetBool("isFalling", true);
            }
        }

        Jump();

        Gravity();
        ProcessWallSlide();
        playerAttack();

    }


    /// <summary>
    /// Adjusts the gravity scale based on the player's vertical velocity.
    /// </summary>
    private void Gravity()
    {
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = baseGravity = fallSpeedMultipler; //Fall increasingly faster
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    /// <summary>
    /// Handles wall sliding when the player is against a wall and moving horizontally.
    /// </summary>
    private void ProcessWallSlide()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        if (OnWall() & horizontal != 0 )
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -wallSlideSpeed));
            PlayerAnimator.SetBool("isWallSliding", true);
        }

        else 
        {
            isWallSliding = false;
            PlayerAnimator.SetBool("isWallSliding", false);
        }
    }

    /// <summary>
    ///Handles wall jumping logic when the player is wall sliding.

    /// </summary>
    private void ProcessWallJump ()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = - transform.localScale.x;
            wallJumpTime = wallJumpTimer;

            CancelInvoke(nameof(CancelWallJump));
        }

        else if (wallJumpTimer > 0f)
        {
            wallJumpTimer -= Time.deltaTime;
        }
    }

    //Fixed update is linked to Unity's physics system so this update is called in step with the physics system rather than frame rate
    private void FixedUpdate()
    {
        if (!isWallJumping)
        {
            Move();
        }
    }

    /// <summary>
    /// move function of player using input Manager
    /// </summary>

    void Move()
    {

        playSmokeFx();
        Flip();
        horizontal = Input.GetAxisRaw("Horizontal");
        Vector2 newVelocity = new Vector2(horizontal * speed, rb.velocity.y);

        if (horizontal != 0)
        {
            PlayerAnimator.SetBool("isWalking", true);
        }

        else
        {
            PlayerAnimator.SetBool("isWalking", false);
        }

        //setting if the platformrb is not null then add the platform's velocity - new velocity
        if (platformRb != null)
        {
            newVelocity += platformRb.velocity;
        }
        
        rb.velocity = newVelocity;

    }

    /// <summary>
    /// Changes sprite direction depending on if you are moving left or right by transforming on the y but 180 degrees
    /// </summary>
    void Flip()
    {
        if (facingRight && horizontal < 0 || !facingRight && horizontal > 0)
        {
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
    }

    /// <summary>
    /// player jump command - referencing key by using "" and unity's Input Manager
    /// </summary>

    void Jump()
    {
            wallJumpPower = new Vector2(wallJumpForce, jumpForce);

        if (Input.GetButtonDown("Jump"))
        {
            if (OnGround())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                PlayerAnimator.SetTrigger("jump");
            }


            else if (OnWall())
            {
                isWallJumping = true;
                wallJumpDirection = facingRight ? 1 : -1;
                rb.velocity = new Vector2(wallJumpDirection * wallJumpPower.x, jumpForce);
                wallJumpTimer = wallJumpTime;
                PlayerAnimator.SetTrigger("jump");


                if (!facingRight)
                {
                    Flip();
                }

                Invoke(nameof(CancelWallJump), wallJumpTime);
            }

            else if (jumpAmount < 1)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpAmount++;
                PlayerAnimator.SetTrigger("jump");
            }
        }
        //Creates variable jump based on when you let go of the jump key, by diving velocity in half when key is released. Players can control the amount you jump by length of key press.
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    /// <summary>
    /// Cancel Wall jumping using bool
    /// </summary>
    private void CancelWallJump()
    {
        isWallJumping = false;
    }

    /// <summary>
    /// boolean value true/false if player is touching "Ground" by using an overlap circle and layers in unity. 
    /// </summary>
    /// <returns></returns>
    private bool OnGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer | platformLayer | movingPlatformLayer);
    }

    /// <summary>
    /// Wall check
    /// </summary>
    /// <returns></returns>
    private bool OnWall()
    {
         return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    /// <summary>
    /// Play's smoke particle FX
    /// </summary>
    private void playSmokeFx()
    { 
        smokeFx.Play();
    }

    /// <summary>
    /// Plays attack ani on click
    /// </summary>
    private void playerAttack()
    {
        if(Input.GetMouseButtonDown(0) && OnGround()) 
        {
            PlayerAnimator.SetBool("isAttacking", true);
        }
    }

    /// <summary>
    /// Handles player attack and damage stats
    /// </summary>
    public void Attack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRadius, enemyLayer);

        foreach (Collider2D enemyGameObject in enemies) 
        {
            RatInteraction ratInteraction = enemyGameObject.GetComponent<RatInteraction>();
            if ( ratInteraction != null)
            {
                ratInteraction.TakeDamage(2);
            }

            SlimeInteraction slimeInteraction = enemyGameObject.GetComponent<SlimeInteraction>();
            if ( slimeInteraction != null ) 
            {
                slimeInteraction.SlimeDamage(2);
            }
        }
    }

    /// <summary>
    /// draws circle to show attack radius
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRadius);
    }

    /// <summary>
    /// Ends player attack animation
    /// </summary>
    public void EndPlayerAttack ()
    {
        PlayerAnimator.SetBool("isAttacking", false);
    }

    /// <summary>
    /// Keeps player momentum locked with platform
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            platformRb = collision.gameObject.GetComponent<Rigidbody2D>();
        }
    }
    /// <summary>
    /// on exit unlock player momentum with platform
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("MovingPlatform"))
        {
            platformRb = null;
        }
    }
}
