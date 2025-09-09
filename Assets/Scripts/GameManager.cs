using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using SimpleJSON;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //----------Events----------//
    public delegate void ScoreUpdatedAction(int score);
    public static event ScoreUpdatedAction OnScoreUpdated;
    
    
    
    //----------Variables----------//

    [SerializeField] private GameObject levelCamera;
    [SerializeField] private GameObject limboBoss;
    [SerializeField] private GameObject lustBoss;
    public static GameManager Instance { get; private set; } 
    
    public int score = 0;
    public int levelsCompleted = 0;
    private bool scoreMultiplier = false; 
    private bool isCoroutineRunning = false;
    private bool IsPlayerAlive = true;
    
    
    
    //----------MonoBehaviour Methods----------//
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
    
    private void OnEnable()
    {
        ScoreCounter.OnScoreCounted += OnScoreCountedHandler;
        PickUpTrigger.OnScoreMultiplierPickUp += OnScoreMultiplierPickUpHandler;
        Platform.OnBossModeTriggered += OnBoss1ModeTriggeredHandler;
        Platform.OnBoss2ModeTriggered += OnBoss2ModeTriggeredHandler;
        Platform.OnLevel1ModeTriggered += OnLevelModeTriggeredHandler;
        Platform.OnLevel2ModeTriggered += OnLevelModeTriggeredHandler;
        ObstacleCollision.OnPlayerDied += OnPlayerDiedHandler;

    }
    
    private void OnDisable()
    {
        ScoreCounter.OnScoreCounted -= OnScoreCountedHandler;
        PickUpTrigger.OnScoreMultiplierPickUp -= OnScoreMultiplierPickUpHandler;
        Platform.OnBossModeTriggered -= OnBoss1ModeTriggeredHandler;
        Platform.OnBoss2ModeTriggered -= OnBoss2ModeTriggeredHandler;
        Platform.OnLevel1ModeTriggered -= OnLevelModeTriggeredHandler;
        Platform.OnLevel2ModeTriggered -= OnLevelModeTriggeredHandler;
        ObstacleCollision.OnPlayerDied -= OnPlayerDiedHandler;
    }

    
    //----------Event Handlers----------//
    
    private void OnScoreCountedHandler()
    {
        if (IsPlayerAlive)
        {
            if (!scoreMultiplier)
            {
                score++;
            }
            else
            {
                score += 2;
            }
        
            OnScoreUpdated?.Invoke(score);
        }
    }

    private void OnScoreMultiplierPickUpHandler()
    {
        
        if (isCoroutineRunning)
        {
            StopAllCoroutines();
        }
        
        StartCoroutine(ScoreMultiplierPickUpTimer());
        
    }

    private void OnBoss1ModeTriggeredHandler()
    {
        levelCamera.GetComponent<Animator>().Play("BossCameraMove");
        limboBoss.SetActive(true);
    }
    
    private void OnBoss2ModeTriggeredHandler()
    {
        levelCamera.GetComponent<Animator>().Play("BossCameraMove");
        lustBoss.SetActive(true);
    }

    
    private void OnLevelModeTriggeredHandler()
    {
        levelsCompleted++;
        levelCamera.GetComponent<Animator>().Play("ReturnToLevelView");
    }
    
    private void OnPlayerDiedHandler()
    {
        IsPlayerAlive = false;
    }
  
    
  

    
    //----------Coroutines----------//
    
    IEnumerator ScoreMultiplierPickUpTimer()
    {
        isCoroutineRunning = true;
        scoreMultiplier = true;
        yield return new WaitForSeconds(5);
        scoreMultiplier = false;
        isCoroutineRunning = true;
    }
    
    
}
