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

    // Change animator values to play animations
    void Animate()
    {
        // If falling then no other animations can be played
        FallingCheck();

        playerAnim.SetBool("isGrounded", isGrounded);
        playerAnim.SetBool("isFalling", isFalling);        
        playerAnim.SetFloat("speed", Mathf.Abs(movement));
    }

    // Movement of the player
    void Move()
    {
        // Get horizontal input
        if(!Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKeyDown(KeyCode.RightArrow))
            movement = Input.GetAxis("Horizontal");

        // Change velocity in dependance of input * speed
        rigidBody.velocity = new Vector2(movement * speed, rigidBody.velocity.y);

        // If player pressed A, then flip the sprite to the left
        if(Input.GetKey(KeyCode.A))
        {
            transform.localScale = new Vector3(-5, 5, 1);
        }

        // If player pressed D, then flip the sprite to the right
        if(Input.GetKey(KeyCode.D))
        {
            transform.localScale = new Vector3(5, 5, 1);
        }
    }

    // Make the player jump
    void Jump()
    {
        // Check if the player is grounded to avoid infinite jumping
        GroundCheck();

        // If spacebar pressed and player is grounded then jump
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
        }
    }

    // Check if player is grounded
    void GroundCheck()
    {
        isGrounded = false;

        // If player collides with ground, then add this object to the array
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);

        // If array isn't empty then player is grounded
        if(colliders.Length > 0)
        {
            isGrounded = true;
            isFalling = false;
        }
    }

    // Check if player is falling
    void FallingCheck()
    {
        // If velocity on y axis is less than 0 and the player isn't grounded then player falls
        if(rigidBody.velocity.y < 0.0f && !isGrounded)
        {
            isFalling = true;
        }
        else
        {
            isFalling = false;
        }
    }

    // What to do when obstacle hit
    void HitObstacle()
    {
        hitsCounter++;

        // If hits counter is less than 3 then play hit animation
        if(hitsCounter <= 2)
        {
            playerAnim.Play("Hit");
        } // Else reset hits counter, respawn player to the checkpoint and play appearing animation
        else
        {
            hitsCounter = 0;

            transform.position = respawnPoint;

            playerAnim.Play("Appearing");
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {   
        // If player collided with obstacle then player is hit
        if(other.gameObject.tag == "Obstacles")
        {
            HitObstacle();
        }   
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // If player triggered by jump platform then add velocity for y axis of the player
        if(other.tag == "JumpPlatform")
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, launchForce);
        }

        // If player triggered by fall detector then respawn player to the checkpoint and play appearing animation
        if(other.tag == "FallDetector")
        {
            transform.position = respawnPoint;

            playerAnim.Play("Appearing");

        }

        // If player triggered by checkpoint then change respawn position
        if(other.tag == "Checkpoint")
        {
            respawnPoint = other.transform.position;
        }

        // If player triggered by obstacles then player is hit
        if(other.tag == "Obstacles")
        {
            HitObstacle();
        }        
    }
}
