using System.Collections;
using System.Collections.Generic;
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

    [Header("Dash")]
    public bool canDash;
    public bool isDashing = false;
    public float dashForce = 14;
    public float dashTime = 0.5f;
    private Vector2 dashDir;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

        if (Input.GetKeyDown(KeyCode.X) && canDash)
        {
            isDashing = true;
            canDash = false;
            dashEffect.emitting = true;
            dashDir = new Vector2(horizontal, vertical);

            if (dashDir == Vector2.zero)
            {
                dashDir = new Vector2(transform.localScale.x, 0);
            }

            //Stop Dash
            StartCoroutine(StopDash());

        }

        if (isDashing == true)
        {
            rb.velocity = dashDir.normalized * dashForce;
            return;
        }


        //If player click X button and canDash
        // if (Input.GetKeyDown(KeyCode.X) && canDash)
        // {
        // StartCoroutine(Dash(new Vector2(horizontal, vertical)));
        // }

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

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
        }
    }

    private bool IsGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    #endregion

    #region Dash


    IEnumerator StopDash()
    {
        yield return new WaitForSeconds(dashTime);
        dashEffect.emitting = false;
        isDashing = false;
    }

    // IEnumerator Dash(Vector2 direction)
    // {
    //     canDash = false;
    //     isDashing = true;
    //     float gravity = rb.gravityScale;
    //     rb.gravityScale = 0;
    //     rb.velocity = new Vector2(direction.x, direction.y) * dashForce;

    //     dashEffect.emitting = true;
    //     yield return new WaitForSeconds(dashTime);
    //     dashEffect.emitting = false;

    //     rb.gravityScale = gravity;
    //     isDashing = false;
    // }
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