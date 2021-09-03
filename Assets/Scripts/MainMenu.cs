using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Vars
    public bool playMusic = true;

    // Components
    public AudioSource music;

    // Executes one time
    void Start()
    {

    }

    public void PlayGame()
    {
        // Goes to the next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        // Quits the game
        Application.Quit();
    }

    public void PlayingMusic()
    {
        // Chagne the state of bool
        playMusic = !playMusic;

        // If true then play music
        if(playMusic)
            music.Play();
        else // Else pause it
            music.Pause();
    }

    public void PlayAgain()
    {
        // Goes to the first scene
        SceneManager.LoadScene(1);
    }
}
