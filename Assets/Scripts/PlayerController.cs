using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private TrailRenderer dashEffect;
    private Rigidbody2D rb;

    [Header("Move")]
    public float moveSpeed = 3;

    [Header("Jump")]
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius;
    public float jumpForce = 10;
    public bool isGrounded;

    [Header("Fall")]
    public float fallMultiplier;
    private float gravityScale;


    [Header("Dash")]
    public bool canDash;
    public bool isDashing = false;
    public float dashForce = 14;
    public float dashTime = 0.5f;
    private Vector2 dashDir;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gravityScale = rb.gravityScale;
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);

        Flip(horizontal);

        //If player grounded
        if (IsGround())
        {
            isGrounded = true;
            canDash = true;
            Jump();
        }

        #region Fall Gravity

        if (rb.velocity.y < 0)
        {
            rb.gravityScale = gravityScale * fallMultiplier;
        }
        else
        {
            rb.gravityScale = gravityScale;
        }

        #endregion


        Restart();
    }

    #region  Movement
    private void Flip(float horizontal)
    {
        if (horizontal > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        if (horizontal < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    #endregion

    #region Jump
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            // rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            // isGrounded = false;
        }
    }

    private bool IsGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    #endregion

    #region Debuging
    private void Restart()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    #endregion
}