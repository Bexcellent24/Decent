using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    
    //----------Events----------//
    public delegate void PlayerDiedAction();
    public static event PlayerDiedAction OnPlayerDied;
    
    public delegate void PlayerLifeLostAction(int livesLost);
    public static event PlayerLifeLostAction OnPlayerLifeLost;

    private bool laserGracePeriodActive = false;
    
    //----------Variables----------//
    [SerializeField] private GameObject thePlayer;
    [SerializeField] private GameObject playerModel;

    private int livesLost = 0;
    //----------MonoBehaviour Methods----------//
    private void OnEnable()
    {
        Obstacle.OnObstacleCollided += OnObstacleCollidedHandler;
        Obstacle.OnLaserCollided += OnLaserCollidedHandler;
        Level1Boss.OnBossDefeated += OnBossDefeatedHandler;
        Level2Boss.OnBoss2Defeated += OnBossDefeatedHandler;
    }

    private void OnDisable()
    {
        Obstacle.OnObstacleCollided -= OnObstacleCollidedHandler;
        Obstacle.OnLaserCollided -= OnLaserCollidedHandler;
        Level1Boss.OnBossDefeated -= OnBossDefeatedHandler;
        Level2Boss.OnBoss2Defeated -= OnBossDefeatedHandler;
    }
    
    
    //----------Event Handlers----------//
    private void OnObstacleCollidedHandler()
    {
        
        OnPlayerDied?.Invoke();
        thePlayer.GetComponent<PlayerController>().enabled = false;
        playerModel.GetComponent<Animator>().Play("Standing Death Backward 01");
        this.enabled = false;

    }
    
    private void OnLaserCollidedHandler()
    {

        if (!laserGracePeriodActive)
        {
            StartCoroutine(LaserColliderSequence());
        }
    }

    private void OnBossDefeatedHandler()
    {
        livesLost = 0;
    }

    //----------Coroutines----------//
    
    private IEnumerator LaserColliderSequence()
    {
        
        laserGracePeriodActive = true;
        livesLost++;
        OnPlayerLifeLost?.Invoke(livesLost);
        Debug.Log(livesLost);
        if (livesLost == 3)
        {
            OnPlayerDied?.Invoke();
        }

        yield return new WaitForSeconds(1);
        laserGracePeriodActive = false;
    }

}
