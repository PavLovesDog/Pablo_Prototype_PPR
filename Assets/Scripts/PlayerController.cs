using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlatformManager platformManager;
    private Transform levelReference;
    private Rigidbody2D rb;
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    private int jumpCount = 0;

    private bool isGrounded;
    private bool isWallSliding;
    private GameObject contactingWall;

    private Vector3 moveVelocity;
    //debug
    public Vector3 jumpVelocity;
    public Vector3 jumpDirection;

    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
        platformManager = FindObjectOfType<PlatformManager>();
        levelReference = platformManager.GetLevel();
    }

    void Update()
    {
        NewMove();
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2 || 
            Input.GetKeyDown(KeyCode.Space) && isWallSliding && jumpCount < 3)
        {
            NewJump();
        }

        Vector3 targetVelocity =jumpVelocity + moveVelocity;
        levelReference.position += targetVelocity*Time.deltaTime;
        if(isWallSliding)
        {
            //display an arrow in the opposing direction from the wall the player is attached to
        }
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        Vector2 targetVelocity = new Vector2(x * moveSpeed, rb.velocity.y);
        rb.velocity = targetVelocity;
    }

    void NewMove()
    {
        //negative because we want the level to move the opposite direction we're pressing
        float x = -Input.GetAxis("Horizontal");

        //get the level and move it in the opposite direction of the move vector
        Vector3 moveDir = new Vector3(x,0.0f,0.0f);
        moveDir.Normalize();
        moveVelocity = moveDir* moveSpeed;
        //levelReference.transform.position += moveDir;
    }

    //moving the level instead of the player
    void NewJump()
    {
        Vector3 jumpDirection = Vector3.zero;
        jumpCount++;
        //TODO: move the level in a parabola shape for the jump
        if(jumpCount ==1)
        {
            jumpDirection = Vector3.down * jumpForce;

        }

        jumpVelocity = jumpDirection;
    }


    void Jump()
    {
        //platformManager.OnPlayerJump(jumpForce);

        jumpCount++; // increment jump counter

        //Vector2 jumpVelocity = Vector2.zero;
        jumpVelocity = Vector2.zero;

        // if its the first jump OR we are wall sliding, add normal force
        if (jumpCount == 1 || isWallSliding)
        {
            //Vector2 jumpDirection = Vector2.up; // Default jump direction is up
            jumpDirection = Vector2.up;

            if (isWallSliding && contactingWall != null)
            {
                // Determine the side of the wall by comparing x positions
                float wallSide = Mathf.Sign(contactingWall.transform.position.x - transform.position.x);

                // Apply jump force upwards and opposite to the wall side
                // (wallSide will be positive if wall is on the right, negative if on the left), 
                jumpDirection += Vector3.right * -wallSide;
                jumpDirection.Normalize();

                //rb.velocity += jumpDirection * 25;

                // Untether wall for next calculation
                contactingWall = null;
            }

            // Apply the jump force with direction
            jumpVelocity += jumpDirection * jumpForce;
        }
        // if second jump and not wallsliding, add fraction of force
        else if (jumpCount == 2 && !isWallSliding)
        {
            //second jump not as powerful
            jumpVelocity = new Vector2(0, jumpForce * 0.75f);
        }

        //add force
        //rb.velocity += jumpVelocity;
    }

    // Check if the player is grounded
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0; // reset jump count
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            isWallSliding = true;
            contactingWall = collision.gameObject; // assign gameobject
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isWallSliding = true;
            //jumpCount = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            isWallSliding = false;
            //contactingWall = null;
        }
    }
}
