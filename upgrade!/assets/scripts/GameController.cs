using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    public GameObject SPGameManager;
    public GameObject CoopGameManager;
    public GameObject PvPGameManager;

    void Awake()
    {
        switch (GameModeManager.gameModeToLoad)
        {
            case GameModes.Single:
                Instantiate(SPGameManager, transform.position, transform.rotation);
                break;
            case GameModes.Coop:
                Instantiate(CoopGameManager, transform.position, transform.rotation);
                break;
            case GameModes.PvP:
                Instantiate(PvPGameManager, transform.position, transform.rotation);
                break;
        }
    }

    // Use this for initialization
    void Start()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {

    }
}

public enum PlayerInput { Move, Jump, Attack, Ability, AltMove, AltJump, AltAttack, AltAbility}
