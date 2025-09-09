using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    
    //----------Variables----------//
    
    [SerializeField] private TextMeshProUGUI scoreTextMesh;
    
    [Header("Levels Completed")]
    [SerializeField] private TextMeshProUGUI levelsCompleted;
    
    [Header("Score Pickup")]
    [SerializeField] private GameObject scoreMultiplierUI;
    [SerializeField] private TextMeshProUGUI scoreMultiplierTimer;
    
    [Header("Speed Pickup")]
    [SerializeField] private GameObject speedMultiplierUI;
    [SerializeField] private TextMeshProUGUI speedMultiplierTimer;
    
     [Header("Immunity Pickup")]
     [SerializeField] private GameObject immunityPickupUI;
     [SerializeField] private TextMeshProUGUI immunityPickupTimer;
     
     [Header("Boss Mode UI")]
     [SerializeField] private GameObject playerLivesUI;
     [SerializeField] private GameObject life1;
     [SerializeField] private GameObject life2;
     [SerializeField] private GameObject life3;
     
     [SerializeField] private GameObject bossModeUI;
     [SerializeField] private GameObject bossLife1;
     [SerializeField] private GameObject bossLife2;
     [SerializeField] private GameObject bossLife3;
     [SerializeField] private GameObject bossLife4;
     [SerializeField] private GameObject bossLife5;
     
     [SerializeField] private GameObject boss2UI;
     [SerializeField] private GameObject bar1;
     [SerializeField] private GameObject bar2;
     [SerializeField] private GameObject bar3;
     [SerializeField] private GameObject bar4;

    private bool scorePickupRunning = false;
    private Coroutine scorePickup;
    
    private bool speedPickupRunning = false;
    private Coroutine speedPickup;
    
    private bool immunityPickupRunning = false;
    private Coroutine immunityPickup;

    private int levelsCompletedSucessfully = -1;
    //----------MonoBehaviour Methods----------//
    
    private void OnEnable()
    {
        GameManager.OnScoreUpdated += OnScoreUpdatedHandler;
        PickUpTrigger.OnScoreMultiplierPickUp += OnScoreMultiplierPickUpHandler;
        PickUpTrigger.OnSpeedMultiplierPickUp += OnSpeedMultiplierPickUpHandler;
        PickUpTrigger.OnImmunityPickedUp += OnImmunityPickedUpHandler;
        Platform.OnBossModeTriggered += OnBossModeTriggeredHandler;
        Platform.OnBoss2ModeTriggered += OnBoss2ModeTriggeredHandler;
        ObstacleCollision.OnPlayerLifeLost += OnPlayerLifeLostHandler;
        Level1Boss.OnBossHit += OnBossPickUpHandler;
        Platform.OnLevel1ModeTriggered += OnLevelModeTriggeredHandler;
        Platform.OnLevel2ModeTriggered += OnLevelModeTriggeredHandler;
        Level2Boss.OnBoss2StageComplete += OnBoss2StageCompleteHandler;
    }
    
    private void OnDisable()
    {
        GameManager.OnScoreUpdated -= OnScoreUpdatedHandler;
        PickUpTrigger.OnScoreMultiplierPickUp -= OnScoreMultiplierPickUpHandler;
        PickUpTrigger.OnSpeedMultiplierPickUp -= OnSpeedMultiplierPickUpHandler;
        PickUpTrigger.OnImmunityPickedUp -= OnImmunityPickedUpHandler;
        Platform.OnBossModeTriggered -= OnBossModeTriggeredHandler;
        Platform.OnBoss2ModeTriggered -= OnBoss2ModeTriggeredHandler;
        ObstacleCollision.OnPlayerLifeLost -= OnPlayerLifeLostHandler;
        Level1Boss.OnBossHit -= OnBossPickUpHandler;
        Platform.OnLevel1ModeTriggered -= OnLevelModeTriggeredHandler;
        Platform.OnLevel2ModeTriggered -= OnLevelModeTriggeredHandler;
        Level2Boss.OnBoss2StageComplete -= OnBoss2StageCompleteHandler;
    }
    
    
    //----------Event Handlers----------//

    private void OnScoreUpdatedHandler(int score)
    {
        scoreTextMesh.text = "Score : " + score; 
    }
    
    private void OnScoreMultiplierPickUpHandler()
    {
        if (scorePickupRunning)
        {
            StopCoroutine(scorePickup);
        }
        scorePickup = StartCoroutine(ScoreMultiplier());
    }

    private void OnSpeedMultiplierPickUpHandler()
    {
        if (speedPickupRunning)
        {
            StopCoroutine(speedPickup);
        }
        speedPickup = StartCoroutine(SpeedMultiplier());
    }

    private void OnImmunityPickedUpHandler()
    {
        if (immunityPickupRunning)
        {
            StopCoroutine(immunityPickup);
        }
        immunityPickup = StartCoroutine(ImmunityPickup());
    }

    private void OnBossModeTriggeredHandler()
    {
        bossModeUI.SetActive(true);
        playerLivesUI.SetActive(true);
    }
    private void OnBoss2ModeTriggeredHandler()
    {
        playerLivesUI.SetActive(true);
        boss2UI.SetActive(true);
    }

    private void OnPlayerLifeLostHandler(int livesLost)
    {
        switch (livesLost)
        {
            case 1: life3.SetActive(false);
                break;
            case 2: life2.SetActive(false);
                break;
            case 3: life1.SetActive(false);
                break;
        }
    }

    private void OnBossPickUpHandler(int bossLife)
    {
        switch (bossLife)
        {
            case 1: bossLife5.SetActive(false);
                break;
            case 2: bossLife4.SetActive(false);
                break;
            case 3: bossLife3.SetActive(false);
                break;
            case 4: bossLife2.SetActive(false);
                break;
            case 5: bossLife1.SetActive(false);
                break;
        }
    }
    
    private void OnBoss2StageCompleteHandler(int stage)
    {
        switch (stage)
        {
            case 1: bar4.SetActive(false);
                break;
            case 2: bar3.SetActive(false);
                break;
            case 3: bar2.SetActive(false);
                break;
            case 4: bar1.SetActive(false);
                break;
        }
    }

    private void OnLevelModeTriggeredHandler()
    {
        bossLife1.SetActive(true);
        bossLife2.SetActive(true);
        bossLife3.SetActive(true);
        bossLife4.SetActive(true);
        bossLife5.SetActive(true);
        life1.SetActive(true);
        life2.SetActive(true);
        life3.SetActive(true);
        bar1.SetActive(true);
        bar2.SetActive(true);
        bar3.SetActive(true);
        bar4.SetActive(true);
        
        boss2UI.SetActive(false);
        bossModeUI.SetActive(false);
        playerLivesUI.SetActive(false);

        levelsCompletedSucessfully++;
        levelsCompleted.text = levelsCompletedSucessfully + "";
    }
    
    //----------Coroutines----------//
    IEnumerator ScoreMultiplier()
    {
        scorePickupRunning = true;
        scoreMultiplierUI.SetActive(true);
        for (int i = 5; i > 0; i--)
        {
            scoreMultiplierTimer.text = "Score X2 : " + i;
            yield return new WaitForSeconds(1);
        }
        scoreMultiplierUI.SetActive(false);
        scorePickupRunning = false;
    }
    
    IEnumerator SpeedMultiplier()
    {
        speedPickupRunning = true;
        speedMultiplierUI.SetActive(true);
        for (int i = 5; i > 0; i--)
        {
            speedMultiplierTimer.text = "Speed X2 : " + i;
            yield return new WaitForSeconds(1);
        }
        speedMultiplierUI.SetActive(false);
        speedPickupRunning = false;
    }
    
    IEnumerator ImmunityPickup()
    {
        immunityPickupRunning = true;
        immunityPickupUI.SetActive(true);
        for (int i = 10; i > 0; i--)
        {
            immunityPickupTimer.text = "Immunity : " + i;
            yield return new WaitForSeconds(1);
        }
        immunityPickupUI.SetActive(false);
        immunityPickupRunning = false;
    }
}
