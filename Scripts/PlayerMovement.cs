using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public ParticleSystem dustEffect;

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float gravityScale = 2f;

    // Double Jump
    private int jumpsLeft;

    // Kill enemy
    [SerializeField] private Transform killPoint;

    // animation
    private enum MovementState { idle, running, jumping, falling, doubleJump }

    // sound
    [SerializeField] private AudioSource jumpSoundEffect;
    [SerializeField] private AudioSource killSoundEffect;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        jumpsLeft = 1;
        killPoint = transform.Find("killPoint");

    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        if (IsGrounded())
        {
            jumpsLeft = 1;
        }

        if (Input.GetButtonDown("Jump") && jumpsLeft > 0)
        {
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpsLeft--;
        }

        UpdateAnimationState();
        Enemy();
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        // Horizontal movement state
        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
            playDustEffect();

        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
            playDustEffect();
        }
        else
        {
            state = MovementState.idle;
        }

        // Vertical movement state
        if (rb.velocity.y > .1f)
        {
            if (jumpsLeft == 1)
            {
                // First jump
                state = MovementState.jumping;
                playDustEffect();
            }
            else if (jumpsLeft == 0)
            {
                // Double jump
                state = MovementState.doubleJump;
            }
        }
        // Falling state
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
            playDustEffect();
        }

        anim.SetInteger("state", (int)state);

    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    private void playDustEffect()
    {
        dustEffect.Play();
    }

    private void Enemy()
    {
        Collider2D enemy = Physics2D.OverlapCircle(killPoint.position, 0.15f, LayerMask.GetMask("Enemy"));

        if (enemy == null)
        {
            return;
        }

        if (rb.velocity.y < 0f && transform.position.y > enemy.transform.position.y + 0.2f)
        {
            killSoundEffect.Play();
            Destroy(enemy.gameObject);

            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(new Vector2(0, 350f));
        }
        else
        {
            GetComponent<PlayerLife>().Die();
        }
    }
}