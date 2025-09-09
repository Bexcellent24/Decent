using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    //----------Variables----------//
    
    [SerializeField] private GameObject pauseMenuUI;
    
    
    //----------MonoBehaviour Methods----------//
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    
    
    //----------Public Custom Methods----------//
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
    }
    
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
    
    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
