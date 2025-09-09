using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuNav;
    [SerializeField] private GameObject leaderboardUI;
    
    //----------Public Custom Methods----------//
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    
    public void ShowLeaderboard()
    {
        mainMenuNav.SetActive(false);
        leaderboardUI.SetActive(true);
    }
    
    public void ReturnToMainMenu()
    {
        leaderboardUI.SetActive(false);
        mainMenuNav.SetActive(true);
    }
}
