using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartSystem : MonoBehaviour
{
    public GameObject[] hearts;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Destory a heart from UI when is called
    public void DestroyHeart()
    {
        for(int i = 0; i<hearts.Length; i++)
        {
            // If first heart met is active then turn it off
            if(hearts[i].gameObject.activeSelf)
            {
                hearts[i].gameObject.SetActive(false);
                break;
            }
        }
    }

    // Restart Hearts and show all of them on the screen
    public void RestartHearts()
    {
        for(int i = 0; i < hearts.Length; i++)
        {
            hearts[i].gameObject.SetActive(true);
        }
    }

    // Destroys all herats
    public void DestroyAllHearts()
    {
        for(int i = 0; i < hearts.Length; i++)
        {
            hearts[i].gameObject.SetActive(false);
        }
    }
}
