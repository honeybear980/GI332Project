using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class PlayerMove : MonoBehaviour
{
    public Rigidbody2D rb;
    bool isFaecingRight = true;
    BoxCollider2D PlayerCollider;
    // movement variables
    public float speed = 5f;
    float moveInput;
    
    // Jump variables
    public float jumpForce = 2f;
    private bool isJump = false;
    // Hold jump variables
    public float HoldDuration = 1f;
    public Image fillImage;
    private float holdTime=0f;
    private bool isHolding = false;
    public float jumpHoldForce = 10f;
    // Ground check variables
    public Transform groundCheck;
    public Vector2 groundCheckRadius = new Vector2(0.5f, 0.5f);
    public LayerMask groundLayer;
    bool isGrounded = false;
    bool isoOnPlatform = false;
    
    
   
    // Gravity variables
    public float baseGravity = 10f;
    public float GravityMultiplier = 2f;
    public float maxgravity = 30f;
    void Start()
    {
        PlayerCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
        Gravity();
        // Check if grounded
        GroundCheck();
        flip();
        
        // Jumphold
        if (isHolding)
        {
            holdTime += Time.deltaTime;
            fillImage.fillAmount = holdTime / HoldDuration;
            Debug.Log(holdTime);
            if (holdTime >= HoldDuration)
            {
                ResetHold();
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        // Visualize ground check in the scene view
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheck.position, groundCheckRadius);
        }
    }
    public void Move (InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>().x;
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded) //  Hold jump
        {
                isHolding = true;
        }
        if (context.canceled && isHolding) // Release jump
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce *(holdTime*jumpHoldForce));
            ResetHold();
        }
    }
    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheck.position, groundCheckRadius, 0, groundLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
    // Method to change the player's scale
    private void flip()
    {
        if (isFaecingRight && moveInput < 0 || !isFaecingRight && moveInput > 0)
        {
            isFaecingRight = !isFaecingRight;
            Vector3 Faecing = transform.localScale;
            Faecing.x *= -1;
            transform.localScale = Faecing;
        }
    }
    private void Gravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravity * GravityMultiplier;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxgravity));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }
    private void ResetHold()
    {
        isHolding = false;
        holdTime = 0f;
        fillImage.fillAmount = 0f;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            isoOnPlatform = true;
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            isoOnPlatform = false;
        }
    }
    private IEnumerator DisablePlayerCollider(float disableTime)
    {
        PlayerCollider.enabled = false;
        yield return new WaitForSeconds(disableTime);
        PlayerCollider.enabled = true;
    }
    
}
