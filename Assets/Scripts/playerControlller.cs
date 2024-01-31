using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControlller : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask jumpableGround;

    private float movementX = 0f;
    [SerializeField] private float moveSpeed = 4.5f;
    [SerializeField] private float jumpHeight = 7f;

    private enum MoveState {idle, running, jumping, falling}

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        movementX = Input.GetAxisRaw("Horizontal"); 
        rb.velocity = new Vector2(movementX * moveSpeed, rb.velocity.y);

        if(Input.GetButtonDown("Jump") && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        }

        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MoveState state;

        if (movementX > 0f)
        {
            state = MoveState.running;
            sprite.flipX = true;
        } else if (movementX < 0f)
        {
            state = MoveState.running;
            sprite.flipX = false;
        } else
        {
            state = MoveState.idle;
        }
        if (rb.velocity.y > .1f)
        {
            state = MoveState.jumping;
        } else if (rb.velocity.y < -.1f)
        {
            state = MoveState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool isGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
