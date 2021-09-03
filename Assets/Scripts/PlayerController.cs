using System.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Components
    private Rigidbody2D rigidBody;
    public Animator playerAnim;
    public LevelManager gameLevelManager;
    public HeartSystem uiHeartSystem;

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

    // Audio
    private AudioSource playerAudio;
    public AudioSource jumpSound;
    public AudioSource damageSound;
    public AudioSource jpSound;
    public AudioSource fruitSound;
    public AudioSource checkpointSound;
    public AudioSource dyingSound;

    // Other
    bool coroutineRunning = false;
    public float hitDelay = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        gameLevelManager = FindObjectOfType<LevelManager>();
        uiHeartSystem = FindObjectOfType<HeartSystem>();
        playerAudio = GetComponent<AudioSource>();
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
            jumpSound.Play();
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
        uiHeartSystem.DestroyHeart();
        damageSound.Play();
        hitsCounter++;

        // If hits counter is less than 3 then play hit animation
        if(hitsCounter <= 2)
        {
            playerAnim.Play("Hit");
        } // Else reset hits counter, respawn player to the checkpoint and play appearing animation
        else
        {
            dyingSound.Play();

            hitsCounter = 0;

            gameLevelManager.Respawn();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // If player triggered by jump platform then add velocity for y axis of the player
        if(other.tag == "JumpPlatform")
        {
            jpSound.Play();
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, launchForce);
        }

        // If player triggered by fall detector then respawn player to the checkpoint and play appearing animation
        if(other.tag == "FallDetector")
        {
            dyingSound.Play();
            uiHeartSystem.DestroyAllHearts();
            gameLevelManager.Respawn();
        }

        // If player triggered by checkpoint then change respawn position
        if(other.tag == "Checkpoint")
        {
            checkpointSound.Play();
            respawnPoint = other.transform.position;
        }    

        // If player triggered by fruit then play sound
        if(other.tag == "Fruit")
        {
            fruitSound.Play();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // If player triggered by an obstacle then player is hit
        if(other.tag == "Obstacle")
        {
            // If coroutine not running then start it, so we have a delay between hits
            if(!coroutineRunning)
                StartCoroutine(HitCoroutine());
        }    
    }

     void OnCollisionStay2D(Collision2D collision)
    {
        // If player collided with an obstacle then player is hit
        if(collision.gameObject.tag == "Obstacle")
        {
            // If coroutine not running then start it, so we have a delay between hits
            if(!coroutineRunning)
                StartCoroutine(HitCoroutine());
        }  
    }

    // Delay after being hit
    IEnumerator HitCoroutine()
    {   
        HitObstacle();
        coroutineRunning = true;
        yield return new WaitForSeconds(hitDelay);
        coroutineRunning = false;
    }
}
