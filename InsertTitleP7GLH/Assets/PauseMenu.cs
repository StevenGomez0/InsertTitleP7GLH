using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel; // Reference to the Pause Panel
    private bool isPaused = false; // Track if the game is paused
    public Button quitButton; // Reference to the Quit Button

    void Start()
    {
        // Add the listener for the Quit button to call the QuitGame method when clicked
        quitButton.onClick.AddListener(QuitGame);
    }

    void Update()
    {
        // Toggle pause state when Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused; // Toggle pause state
        pausePanel.SetActive(isPaused); // Show or hide the pause panel

        // Pause or resume the game
        if (isPaused)
        {
            Time.timeScale = 0f; // Pause the game
        }
        else
        {
            Time.timeScale = 1f; // Resume the game
        }
    }

    // Method to quit the game
    void QuitGame()
    {
#if UNITY_EDITOR
        // If running in the editor, stop the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // If running in a build, quit the game
        Application.Quit();
#endif
    }
}
