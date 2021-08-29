using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    private Animator checkpointAnim;
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
        if(other.tag == "Player" && !checkpointReached)
        {
            checkpointAnim.Play("CheckpointBeingTaken");
            checkpointReached = true;
        }
    }
}
