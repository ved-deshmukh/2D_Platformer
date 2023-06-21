using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private float dirX=0;
    private SpriteRenderer sprite;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpForce = 20f;
    private BoxCollider2D col;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private AudioSource jumpSound;
    bool isTouchingFront;
    public Transform frontCheck;
    bool wallSliding;
    public float wallSlidingSpeed;
    
    bool wallJumping;
    public float xWallForce;
    public float yWallForce;
    public float wallJumpTime;
    [SerializeField] private float circleSize;

    private enum MovementState { idle, running, jumping, falling, sliding }      
    // Start is called before the first frame update
    private void Start()
    {
        rb= GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        col= GetComponent<BoxCollider2D>();
        frontCheck = GetComponent<Transform>();
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            jumpSound.Play();
            rb.velocity = new Vector3(0, jumpForce, 0);
        }
        UpdateAnimationState();
        isTouchingFront = Physics2D.OverlapCircle(frontCheck.position, circleSize , jumpableGround);
        if(isTouchingFront && !IsGrounded()  && rb.velocity.y <-0.1f)
        {
            if(dirX != 0)
            wallSliding = true;
        }
        else
        {
            wallSliding = false;
        }
        if(wallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            if (Input.GetButtonDown("Jump"))
                wallJumping = true;
        }
        if(wallJumping)
        {
            rb.velocity = new Vector2(xWallForce*(sprite.flipX?1:-1), yWallForce);
            Invoke("WallJumpingStop", wallJumpTime);
        }
    }

    private void UpdateAnimationState()
    {
        MovementState state;
        if (dirX > 0f && !wallJumping)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f && !wallJumping)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }
        if (rb.velocity.y > 0.1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }
        if (wallSliding)
        {
            state = MovementState.sliding;
        }
        anim.SetInteger("state", (int)state );
        
    }
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
    void WallJumpingStop()
    {
        wallJumping = false;
    }
}
