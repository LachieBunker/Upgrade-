using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

    //public string currentGameMode;//gameModeToLoad

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void SetCurrentGameMode(string gameMode)
    {
        //Debug.Log(gameMode);
        switch (gameMode)
        {
            case "Single":
                GameModeManager.gameModeToLoad = GameModes.Single;
                break;
            case "Coop":
                GameModeManager.gameModeToLoad = GameModes.Coop;
                break;
            case "PvP":
                GameModeManager.gameModeToLoad = GameModes.PvP;
                break;
        }
        
    }

    public void PlayGame(int map)
    {
        SceneManager.LoadScene(map);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
