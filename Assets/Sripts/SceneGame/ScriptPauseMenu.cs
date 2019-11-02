using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptPauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject PauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        //Desactive le menu pour le cacher pour le joueur
        PauseMenuUI.SetActive(false);
        //Le temps s'écoule normalement
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        //On réaffiche le menu
        PauseMenuUI.SetActive(true);
        //On arrete le temps du jeu
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        //Changement de scène
        SceneManager.LoadScene("SceneMenu");
    }

    public void QuitGame()
    {
        //Le jeu se ferme si on est en build
        Application.Quit();
    }
}
