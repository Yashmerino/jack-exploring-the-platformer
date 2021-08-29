using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlatformController : MonoBehaviour
{
    // Components
    private Animator jpAnim;

    // Start is called before the first frame update
    void Start()
    {
        jpAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // If jump platform triggered by player then play animation
        if(other.tag == "Player")
        {
            jpAnim.Play("Jump");
        }
    }
}
