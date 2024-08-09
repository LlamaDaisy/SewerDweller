using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRatChase : MonoBehaviour
{
    [SerializeField]
    Transform playerTarget;
    GameObject player;

    [SerializeField]
    float chaseSpeed;
    [SerializeField]
    float jumpForce;
    [SerializeField]
    float chaseDistance;

    [SerializeField]
    LayerMask groundLayer;
    [SerializeField]
    Transform groundCheck;
    [SerializeField]
    LayerMask platformLayer;

    Rigidbody2D rb;

    bool facingRight = false;
    bool canMove = true;

    FloatingHealthBar updateHealthBar;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        updateHealthBar = GetComponentInChildren<FloatingHealthBar>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void  FixedUpdate()
    {
        //calculating the distance between the enemy and player
        float playerDistance = Vector2.Distance(transform.position, player.transform.position);

        //calculating the horizontal distance
        float horizontalDistance = player.transform.position.x - transform.position.x;
        
        //calculating the verticle distance
        float verticleDistance = player.transform.position.y - transform.position.y;

        //if player is less than chase distance & on ground
        if (playerDistance < chaseDistance && OnGround() && canMove)
        {
           
            //sets the direction of the enemy
            float direction = Mathf.Sign(playerTarget.position.x - transform.position.x);

            if ((facingRight && direction < 0) || (!facingRight && direction > 0))
            {
                Flip();
            }

            //Checks if the absolute vertical distance between the player and the enemy is less than 1 unit,
            //meaning they are on roughly the same level.
            if (Mathf.Abs(verticleDistance) < 1f)
            {
                rb.velocity = new Vector2(direction * chaseSpeed, rb.velocity.y);
            }

            //Checks if the absolute horizontal distance between the player and the enemy is greater than or equal to 1 unit if yes than jump.
            else if (Mathf.Abs(horizontalDistance) >= 1f)
            {
                Jump();
            }

            //Jumping up or down to the player's position if the vertical distance is greater than or equal to 1
            else
            {
                rb.velocity = new Vector2(direction * chaseSpeed, Mathf.Sign(verticleDistance) * chaseSpeed);
            }

        }
        else
        {
                rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2 (rb.velocity.x, 3 * jumpForce);
    }

    bool OnGround()
    {
        // Define the size of the box (width and height)
        Vector2 boxSize = new Vector2(0.3f, 0.005f); // Adjust the size as necessary

        // Calculate the center position of the box
        Vector2 boxCenter = transform.position;

        // Check for colliders within the box area
        Collider2D collider = Physics2D.OverlapBox(boxCenter, boxSize, 0f, groundLayer | platformLayer);

        // Return true if a collider is found, indicating the player is on the ground
        return collider != null;
    }

    /// <summary>
    /// Flips sprite based on direction
    /// </summary>
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        // Adjust health bar flip state
        if (updateHealthBar != null)
        {
            updateHealthBar.SetFlipped(facingRight);
        }
    }

    /// <summary>
    /// stops the rat moving if collided with player
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canMove = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canMove = true;
        }
    }
}

//chase the player if the player comes into a min distance of the enemy
//my - done
//chase the player along the ground - done
//attack the player if the player enters the collider - will be in the RatInteraction script - done
//follow the player up/down a platform if it is => - down only but im happy with that
//stop and idle under the player if the enemy can no longer reach - kind, little hops are v cute!
//stop chasing if player jump to a new platform - this is covered in playerDistance
