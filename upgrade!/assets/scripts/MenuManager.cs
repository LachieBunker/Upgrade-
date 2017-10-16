using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public GameState gameState;

    public GameObject pauseCanvas;
    public GameObject gameOverCanvas;

	// Use this for initialization
	void Start ()
    {
        gameState = GameState.Playing;
        Time.timeScale = 1.0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameState == GameState.Playing)
            {
                Pause();
            }
            else if (gameState == GameState.Paused)
            {
                Resume();
            }
        }
	}

    public void Pause()
    {
	    if(gameState == GameState.Playing)
	    {
			gameState = GameState.Paused;
	        Time.timeScale = 0.0f;
	        pauseCanvas.SetActive(true);
	    }
    }

    public void Resume()
    {
        gameState = GameState.Playing;
        Time.timeScale = 1.0f;
        pauseCanvas.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    public void GameLost()
    {
        Time.timeScale = 0.0f;
        gameOverCanvas.SetActive(true);
    }
}

public enum GameState { Playing, Paused, Lost}
