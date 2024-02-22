using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashDuration;
    [SerializeField] GroundDetection groundDetection;

    public Rigidbody2D rb;
    bool jumpPressed = false;
    bool canDash = false;
    bool dashing = false;
    Vector2 dashDirection;

    public event Action VelocityModifiers;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = new(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"));

        float moveVelocity = direction.x * speed;

        rb.velocity = new(moveVelocity, rb.velocity.y);

        float jumpAxis = Input.GetAxisRaw("Jump");
        if (groundDetection.onGround && jumpAxis > 0 && !jumpPressed)
        {
            jumpPressed = true;
            Jump();
        }
        if (jumpAxis == 0)
        {
            jumpPressed = false;
        }

        if (!dashing && canDash && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)))
        {
            Dash(direction);
        }
    }

    void Dash(Vector2 direction)
    {
        dashDirection = direction.normalized;
        dashing = true;
        canDash = false;
        StartCoroutine(DashTimer());
    }

    IEnumerator DashTimer()
    {
        yield return new WaitForSeconds(dashDuration);
        rb.velocity = Vector2.zero;
        dashing = false;
    }

    private void FixedUpdate()
    {
        if (dashing)
        {
            rb.velocity = dashDirection * dashSpeed;
        }
        if (groundDetection.onGround)
        {
            canDash = true;
        }

        VelocityModifiers?.Invoke();
    }

    void Jump()
    {
        rb.AddForce(new Vector2(0, jumpForce));
    }

    public void ClearVelocityModifiers()
    {
        VelocityModifiers = null;
    }
}
