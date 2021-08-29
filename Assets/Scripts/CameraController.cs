using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Offsets
    public float offsetX = 3.0f;
    public float offsetY = 2.0f;
    public float offsetSmoothing = 2.0f;

    // Components
    public GameObject player;

    // Vars
    private Vector3 playerPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Initialize playerPosition var
        playerPosition = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);

        // Change offset x in dependance of in which direction is player looking
        if(player.transform.localScale.x > 0.0f)
        {
            // If player is looking right direction
            playerPosition = new Vector3(playerPosition.x + offsetX, playerPosition.y, playerPosition.z);
        }
        else
        {   // If player is looking left direction
            playerPosition = new Vector3(playerPosition.x - offsetX, playerPosition.y, playerPosition.z);
        }

        // Add offset to y axis
        transform.position = new Vector3(transform.position.x, player.transform.position.y + offsetY, transform.position.z);
        // Smooth following of the player
        transform.position = Vector3.Lerp(transform.position, playerPosition, offsetSmoothing * Time.deltaTime);
    }
}
