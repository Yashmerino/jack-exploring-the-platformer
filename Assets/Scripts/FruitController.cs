using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitController : MonoBehaviour
{
    // Components
    private Animator fruitAnim;
    private LevelManager gameLevelManager;

    // Start is called before the first frame update
    void Start()
    {
        fruitAnim = GetComponent<Animator>();
        gameLevelManager = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // After collected animation played destroy objet
    void DestroyOnExit()
    {
        Destroy(gameObject, 0.6f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // If fruit triggered by player then play animation
        if(other.tag == "Player")
        {
            //Add 1 score when fruit collected
            gameLevelManager.AddFruits(1);

            fruitAnim.Play("Collected");
            DestroyOnExit();
        }
    }
}
