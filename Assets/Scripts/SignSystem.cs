using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SignSystem : MonoBehaviour
{
    // Vars
    public string tipString;
    public Image tipBox;
    public TextMeshProUGUI tipText;

    // Start is called before the first frame update
    void Start()
    {
        // Disable tip on the start
        tipText.enabled = false;
        tipBox.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Set text for the tip on the screen
    void setTipText()
    {
        // Set text from string
        tipText.text = tipString;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // If sign triggered by player then enable tip
        if(other.tag == "Player")
        {
            setTipText();
            tipBox.enabled = true;
            tipText.enabled = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            // If sign triggered by player then disable tip
            tipBox.enabled = false;
            tipText.enabled = false;
        }
    }
}
