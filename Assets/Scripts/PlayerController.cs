using System.Collections;
using UnityEditor;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;

/// <summary>
/// 
/// TODO:
/// 2 upgrades
/// 
/// </summary>
public class PlayerController : MonoBehaviour
{
    PlatformManager platformManager;
    private Transform levelReference;
    private Rigidbody2D rb;
    private Animator animator;
    public float scrollSpeed = 5.0f;
    public float moveSpeed = 5f;
    public float dashSpeed = 10f;
    public float dashCooldown = 1f;
    private bool canDash = true;
    private bool pauseMovementInput = false;
    public float jumpForce = 5f;
    private int jumpCount = 0;

    private bool isGrounded;
    private bool isWallSliding;
    private bool levelUpdated = true;
    private bool hasJumpBoots = false;
    public Image jumpUpgradeSprite;
    private bool hasDashBoots = false;
    public Image dashUpgradeSprite;
    private GameObject contactingWall;

    //Health
    public int currentHitsLeft;
    public int maxHits = 3;

    //TESTING - scoring bool
    public bool isScoring = false;

    private Vector2 moveVelocity;
    //debug
    public Vector2 jumpVelocity;
    public Vector2 jumpDirection;

    SpriteRenderer sprite;
    bool canResetIsGrounded = true;

    void Start()
    {

        //set health full
        currentHitsLeft = maxHits;

        rb = GetComponent<Rigidbody2D>();
        platformManager = FindObjectOfType<PlatformManager>();
        levelReference = platformManager.GetLevel();
        jumpVelocity = Vector2.zero;

        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        //checkl for dead conditions
        CheckDeath();

        //dashing
        if ((Input.GetKeyDown(KeyCode.LeftShift) && canDash) && hasDashBoots)
        {
            pauseMovementInput = true;
            Dash();
        }
        else if (!pauseMovementInput)
        {
            Move();
        }
        //lets double jump be available

        if (isGrounded || hasJumpBoots)
        {

            // Check if spacebar is currently being held down
            if (Input.GetKey(KeyCode.Space) && isGrounded)
            {
                canResetIsGrounded = false;
                Jump();
                StartCoroutine(ResetCanResetIsGroundedBool());
            }
            else if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2)
            {
                // Allows for a single double jump if the player is in the air and hasn't double jumped yet
                Jump();
            }
        }


        Vector2 targetVelocity = jumpVelocity + moveVelocity;
        /*
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            Dash();
        }
        */
        UpdatePosition();
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        Vector2 targetVelocity = new Vector2(x * moveSpeed, rb.velocity.y);

        //sprite direction
        if (targetVelocity.x > 0)
        {
            sprite.flipX = false;
            animator.SetBool("isWalking", true);
        }
        else if (targetVelocity.x < 0)
        {
            sprite.flipX = true;
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        rb.velocity = targetVelocity;
    }

    void Dash()
    {
        canDash = false;
        // apply a impulse force in the direction the player is pressing

        float x = Input.GetAxisRaw("Horizontal");
        Vector2 impulseDirection = new Vector2(x * dashSpeed, 0);

        rb.AddForce(impulseDirection, ForceMode2D.Impulse);

        StartCoroutine(ResetDashBool());
    }

    IEnumerator ResetDashBool()
    {
        yield return new WaitForSeconds(0.25f);
        pauseMovementInput = false;

        yield return new WaitForSeconds(dashCooldown / 2);
        canDash = true;
    }

    void Jump()
    {
        // Reset vertical velocity to ensure consistent jump behavior
        rb.velocity = new Vector2(rb.velocity.x, 0);

        // If the player is grounded or this is the first jump in the air (for double jumping)
        if (isGrounded || (jumpCount < 2))
        {
            float force = jumpForce;

            // If not grounded and attempting a double jump, reduce the force
            if (!isGrounded && jumpCount == 1)
            {
                force *= 0.75f; // Reduce force for double jump
            }

            // Apply jump force
            rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);

            // Update jumpCount and isGrounded status
            jumpCount++;
            if (isGrounded)
            {
                isGrounded = false; // Mark as no longer grounded since we're jumping
                jumpCount = 1; // Reset jumpCount to allow for the next jump
            }
        }
    }

    IEnumerator ResetCanResetIsGroundedBool()
    {
        yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds before allowing another jump
        canResetIsGrounded = true;
    }
    private void UpdatePosition()
    {

        if (/*isGrounded&&*/transform.position.y > -1)
        {
            levelReference.Translate(Vector3.down * scrollSpeed * Time.deltaTime);
            transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

            isScoring = true;
        }
        else if (transform.position.y < -3.5f) // below camera range, scroll back down a little
        {
            levelReference.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
            transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
        }
        else
        {
            levelUpdated = true;
            isScoring = false;
        }
    }

    // Check if the player is grounded
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //PLAYER HIT BY DAMAGEABLE
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            DeductHealth();
            //minus some points
        }

        //Debug.Log(collision.gameObject.name);
        //if the collided object is ground (i.e. platform) and if the platform is below player
        if (collision.gameObject.CompareTag("Ground") && collision.transform.position.y < transform.position.y)
        {
            if (canResetIsGrounded)
                isGrounded = true;
            jumpCount = 0; // reset jump count
            levelUpdated = false;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            isWallSliding = true;
            contactingWall = collision.gameObject; // assign gameobject
        }
    }

    private void DeductHealth()
    {
        currentHitsLeft -= 1;
    }
    private void CheckDeath()
    {
        //chcek health
        if (currentHitsLeft <= 0)
        {
            // dead, end game
            Debug.Log("DEATH!");
            EndGame();
        }
        //if the player is below the camera
        if (transform.position.y < Camera.main.transform.position.y - (Camera.main.orthographicSize / 2 + 6))
        {
            print("death");
            //Checks wheehter it's played in unity editor or standalone. This is only for quitting so far
            EndGame();
        }
    }
    private void EndGame()
    {
#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    // COLLISION STUFF
    #region Collision
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Upgrade")
        {
            if (collision.name == "JumpBoots")
            {
                //change alpha of jump boot image 
                Color currentcolor = jumpUpgradeSprite.color;
                currentcolor.a = 1f;
                jumpUpgradeSprite.color = currentcolor;
                hasJumpBoots = true;
                Destroy(collision.gameObject);
            }
            else if (collision.name == "DashBoots")
            {
                //change alpha of dash boot image 
                Color currentcolor = dashUpgradeSprite.color;
                currentcolor.a = 1f;
                dashUpgradeSprite.color = currentcolor;
                hasDashBoots = true;
                
                Destroy(collision.gameObject);
            }
        }
    }
    #endregion

}

