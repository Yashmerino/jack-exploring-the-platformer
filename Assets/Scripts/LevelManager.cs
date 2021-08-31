using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    // Components
    public PlayerController gamePlayer;
    public Text scoreText;
    public HeartSystem uiHeartSystem;

    // Vars
    public float respawnDelay = 2.0f;
    public int fruits = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Find the object with "PlayerController" script
        gamePlayer = FindObjectOfType<PlayerController>();
        uiHeartSystem = FindObjectOfType<HeartSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Respawns the player at last checkpoint
    public void Respawn()
    {
        StartCoroutine("RespawnCoroutine");
    }

    public IEnumerator RespawnCoroutine()
    {
        // Player not active anymore
        gamePlayer.gameObject.SetActive(false);

        // Wait for 2 seconds before doing other stuff
        yield return new WaitForSeconds(respawnDelay);

        // Change player position to respawn point
        gamePlayer.transform.position = gamePlayer.respawnPoint;

        // Play appearing animation
        gamePlayer.playerAnim.Play("Appearing");
        // Set the player active again
        gamePlayer.gameObject.SetActive(true);
        // Restart hearts
        uiHeartSystem.RestartHearts();
    }

    // Add fruits when a fruit is collected
    public void AddFruits(int numberOfFruits)
    {
        fruits += numberOfFruits;
        // Change text to current amount of collected fruits
        scoreText.text = "Fruits: " + fruits;
    }
}
