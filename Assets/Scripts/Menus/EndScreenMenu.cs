using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class EndScreenMenu : MonoBehaviour
{
    //----------Variables----------//
    
    [SerializeField] private TextMeshProUGUI finalScore;
    [SerializeField] private GameObject endScreen;
    
    //----------MonoBehaviour Methods----------//
    private void OnEnable()
    {
        GameManager.OnScoreUpdated += OnScoreUpdatedHandler;
        ObstacleCollision.OnPlayerDied += OnPlayerDiedHandler;
    }
    
    private void OnDisable()
    {
        GameManager.OnScoreUpdated -= OnScoreUpdatedHandler;
        ObstacleCollision.OnPlayerDied -= OnPlayerDiedHandler;
    }
    
    
    //----------Event Handlers----------//

    private void OnScoreUpdatedHandler(int score)
    {
        finalScore.text = "Score : " + score;
    }
    
    
    //----------Public Custom Methods----------//
    public void TryAgain()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    //----------Event Handlers----------//
    private void OnPlayerDiedHandler()
    {
        StartCoroutine(EndSeqence());
    }
    
    
    
    //----------Coroutines----------//

    IEnumerator EndSeqence()
    {
        yield return new WaitForSeconds(3);
        endScreen.SetActive(true);
    }
}
