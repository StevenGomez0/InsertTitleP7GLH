using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KillCounter : MonoBehaviour
{
    public TMP_Text killCounterText;
    public int totalEnemies = 10;  
    private int enemiesKilled = 0; 

    void Start()
    {
        // Initialize the kill counter display
        UpdateKillCounter();
    }

    // Call this method whenever an enemy is killed
    public void OnEnemyKilled()
    {
        enemiesKilled++;
        UpdateKillCounter();

        // Check if all enemies are killed
        if (enemiesKilled >= totalEnemies)
        {
            UnityEngine.Debug.Log("All enemies defeated!");
        }
    }

    // Update the counter text display
    void UpdateKillCounter()
    {
        killCounterText.text = $"Enemies Killed: {enemiesKilled} / {totalEnemies}";
    }
}
