using System.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Components
    private Rigidbody2D rigidBody;

    private Animator playerAnim;

    // Movement vars
    public float speed = 6.0f;
    public float jumpForce = 8.0f;
    public float launchForce = 20.0f;
    private float movement;

    // Ground checking
    private bool isGrounded = false;
    private bool isFalling = false;
    public Transform groundCheckCollider;
    const float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    // Respawn
    public Vector3 respawnPoint;
    private int hitsCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        Animate();
    }

    void Animate()
    {
        FallingCheck();

        playerAnim.SetBool("isGrounded", isGrounded);
        playerAnim.SetBool("isFalling", isFalling);        
        playerAnim.SetFloat("speed", Mathf.Abs(movement));
    }

    void Move()
    {
        if(!Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKeyDown(KeyCode.RightArrow))
            movement = Input.GetAxis("Horizontal");

        rigidBody.velocity = new Vector2(movement * speed, rigidBody.velocity.y);

        if(Input.GetKey(KeyCode.A))
        {
            transform.localScale = new Vector3(-5, 5, 1);
        }

        if(Input.GetKey(KeyCode.D))
        {
            transform.localScale = new Vector3(5, 5, 1);
        }
    }

    void Jump()
    {
        GroundCheck();

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
        }
    }

    void GroundCheck()
    {
        isGrounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);

        if(colliders.Length > 0)
        {
            isGrounded = true;
            isFalling = false;
        }
    }

    void FallingCheck()
    {
        if(rigidBody.velocity.y < 0.0f && !isGrounded)
        {
            isFalling = true;
        }
        else
        {
            isFalling = false;
        }
    }

    void HitObstacle()
    {
        hitsCounter++;

        if(hitsCounter <= 2)
        {
            playerAnim.Play("Hit");
        }
        else
        {
            hitsCounter = 0;

            transform.position = respawnPoint;

            playerAnim.Play("Appearing");
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Obstacles")
        {
            HitObstacle();
        }   
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "JumpPlatform")
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, launchForce);
        }

        if(other.tag == "FallDetector")
        {
            transform.position = respawnPoint;

            playerAnim.Play("Appearing");

        }

        if(other.tag == "Checkpoint")
        {
            respawnPoint = other.transform.position;
        }

        if(other.tag == "Obstacles")
        {
            HitObstacle();
        }        
    }
}
