using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    // Components
    private Animator checkpointAnim;

    // Vars
    private bool checkpointReached = false;

    // Start is called before the first frame update
    void Start()
    {
        checkpointAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // If checkpoint triggered by play and checkpoint hasn't been reached before then play animation 
        if(other.tag == "Player" && !checkpointReached)
        {
            checkpointAnim.Play("CheckpointBeingTaken");
            checkpointReached = true;
        }
    }
}
