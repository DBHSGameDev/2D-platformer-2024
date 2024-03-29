using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance; // for easy reference from other gameObjects

    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashDuration;
    // the factor multiplied to gravity scale for player releasing the jump button
    [SerializeField, Range(1,10)] float fastFallFactor; 
    [SerializeField] GroundDetection groundDetection; // GroundDetection component

    [NonSerialized] public Rigidbody2D rb;

    bool jumpPressed = false;

    bool canDash = false;
    bool dashing = false;
    Vector2 dashDirection;

    bool fastFall = false;

    public event Action VelocityModifiers; // actions (methods) run to modify the velocity every physic frame

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // 2D direction
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

    // reset dashing and velocity
    IEnumerator DashTimer()
    {
        yield return new WaitForSeconds(dashDuration);
        rb.velocity = Vector2.zero;
        dashing = false;
    }

    // FixedUpdate is a frame-rate independent message called once before every physic calculation
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
        else if (Input.GetAxisRaw("Jump") == 0)
        {
            SetFastFall(true);
        }

        VelocityModifiers?.Invoke();
    }

    void Jump()
    {
        rb.AddForce(new Vector2(0, jumpForce));
    }

    // message from GroundDetection
    public void OnLanded()
    {
        SetFastFall(false);
    }

    // allow for clear the event from external sources
    public void ClearVelocityModifiers()
    {
        VelocityModifiers = null;
    }

    // set the fast fall field and modify gravity scale
    void SetFastFall(bool value)
    {
        if (value == fastFall) return;
        fastFall = value;
        if (fastFall)
        {
            rb.gravityScale *= fastFallFactor;
        }
        else
        {
            rb.gravityScale /= fastFallFactor;
        }
    }
}
