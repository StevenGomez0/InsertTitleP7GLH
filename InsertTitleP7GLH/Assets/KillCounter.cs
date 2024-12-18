using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KillCounter : MonoBehaviour
{
    //i realize this would probably just work better if i threw all the wave system code onto here instead of making another script
    //so im doing that
    private float spawnRangeMin = 35;
    private float spawnRangeMax = 45;
    public TMP_Text killCounterText;
    private int totalEnemies;  
    private int enemiesKilled = 0;
    private int EnemiesToSpawn;
    private int WaveCount = 0;
    public GameObject enemyPrefab;

    void Start()
    {
        // Initialize the kill counter display
        UpdateKillCounter();
        if (EnemiesToSpawn == 0) EnemiesToSpawn = 1;
        SpawnEnemyWave(EnemiesToSpawn);
    }

    public Vector3 GenerateSpawnPos()
    { //icky spaghetti code
        float xPos = Random.Range(spawnRangeMin, spawnRangeMax);
        while (xPos > 38 && xPos < 42) xPos = Random.Range(spawnRangeMin, spawnRangeMax); //if the enemy would spawn in a pillar, reroll the range until not in pillar
        if(Random.Range(0,2)*2-1 == -1) // 50/50 to make pos negative
        {
            xPos = -xPos;
        }
        float zPos = Random.Range(spawnRangeMin, spawnRangeMax); //ditto every comment
        while (zPos > 38 && zPos < 42) zPos = Random.Range(spawnRangeMin, spawnRangeMax);
        if (Random.Range(0, 2) * 2 - 1 == -1)
        {
            zPos = -zPos;
        }
        Vector3 randomPos = new Vector3(xPos, 1, zPos);
        return randomPos; 
    }

    void SpawnEnemyWave(int enemiesSpawning)
    {
        UnityEngine.Debug.Log("Spawning enemies, " + enemiesSpawning + " total.");
        totalEnemies = enemiesSpawning;
        enemiesKilled = 0;
        for (int i = 0; i < enemiesSpawning; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPos(), Quaternion.identity);
        }
    }

    // Call this method whenever an enemy is killed
    public void OnEnemyKilled()
    {
        enemiesKilled++;
        UpdateKillCounter();

        // Check if all enemies are killed
        if (enemiesKilled >= totalEnemies)
        {
            WaveCount++;
            EnemiesToSpawn = WaveCount * 3;
            SpawnEnemyWave(EnemiesToSpawn);
            UpdateKillCounter();
            UnityEngine.Debug.Log("All enemies defeated!"); //why is this ambiguous when not defined by unityengine???? confusing
        }
    }

    // Update the counter text display
    void UpdateKillCounter()
    {
        killCounterText.text = $"Enemies Killed: {enemiesKilled} / {totalEnemies}";
    }
}
