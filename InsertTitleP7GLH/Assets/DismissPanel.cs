using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DismissPanel : MonoBehaviour
{
    public GameObject introPanel;       
    public GameObject secondPanel;      
    public GameObject killCounterPanel;
    public TMP_Text killCounterText;
    public int totalEnemies = 10;        
    private int enemiesKilled = 0;       

    private bool isFirstPanelActive = true;
    private bool isSecondPanelActive = false;

    void Start()
    {
       
        introPanel.SetActive(true);
        secondPanel.SetActive(false);
        killCounterPanel.SetActive(false);
    }

    void Update()
    {
      
        if (isFirstPanelActive && Input.GetKeyDown(KeyCode.Space))
        {
            introPanel.SetActive(false);
            secondPanel.SetActive(true);
            isFirstPanelActive = false;
            isSecondPanelActive = true;
        }
     
        else if (isSecondPanelActive && Input.GetKeyDown(KeyCode.Space))
        {
            secondPanel.SetActive(false);
            killCounterPanel.SetActive(true);
            isSecondPanelActive = false;
        }
    }

    
    /*public void OnEnemyKilled()
    {
        enemiesKilled++;
        UpdateKillCounter();

        if (enemiesKilled >= totalEnemies)
        {
            UnityEngine.Debug.Log("All Enemies Defeated!");
        }
    }

   
    void UpdateKillCounter()
    {
        killCounterText.text = $"Enemies Killed: {enemiesKilled} / {totalEnemies}";
    } 
    not sure why this is here, commenting out for now */
}
