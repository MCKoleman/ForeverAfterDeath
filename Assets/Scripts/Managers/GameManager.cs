using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SceneLoader))]
public class GameManager : Singleton<GameManager>
{
    public enum GameState { INVALID = 0, MENU = 1, PAUSED = 2, LOADING_LEVEL = 3, GENERATING_LEVEL = 4, IN_GAME = 5 }

    [SerializeField]
    private GameState gameState;
    [SerializeField]
    private GameState prevGameState;
    public GameState State { get { return gameState; } }
    public bool IsGameActive { get; private set; }

    [SerializeField]
    private bool isEasyMode = false;
    [SerializeField]
    private bool DEBUG_DISABLE_LEVEL = false;

    private SceneLoader sceneLoader;
    private bool isReady = false;
    [SerializeField]
    private int trialNum = 1;

    // Initialize all other singletons
    void Start()
    {
        sceneLoader = this.GetComponent<SceneLoader>();
        gameState = GameState.INVALID;
        prevGameState = GameState.INVALID;
        this.Init();
    }

    // Initializes the game manager and all singletons
    public void Init()
    {
        PrefabManager.Instance.Init();
        AudioManager.Instance.Init();
        GenManager.Instance.Init();
        UIManager.Instance.Init();
        InputManager.Instance.Init();
        isReady = true;
    }

    // Starts the game
    public void StartGame()
    {
        SetIsGameActive(true);
        UIManager.Instance.InitHUD();
        SetTimeScale(1.0f);
        Print.Log("Started game");
    }

    // Ends the game 
    public void EndGame()
    {
        Print.Log("Ended game");
        SetIsGameActive(false);
        UIManager.Instance.ShowDeathMenu();
    }

    // Restarts the game
    public void RestartGame()
    {
        this.Init();
        IncTrialNum();
        HandleLevelSwap(1);
    }

    // Communicates scene changing to necessary components, such as the level generator
    public void HandleSceneChange(GlobalVars.SceneType sceneType)
    {
        switch (sceneType)
        {
            case GlobalVars.SceneType.MENU:
                SetGameState(GameState.MENU);
                break;
            case GlobalVars.SceneType.TUTORIAL:
                SetGameState(GameState.IN_GAME);
                SetIsGameActive(true);
                break;
            case GlobalVars.SceneType.TEST:
                SetGameState(GameState.IN_GAME);
                SetIsGameActive(true);
                break;
            case GlobalVars.SceneType.LEVEL:
            case GlobalVars.SceneType.MINIBOSS:
            case GlobalVars.SceneType.BOSS:
#if UNITY_EDITOR
                if (DEBUG_DISABLE_LEVEL)
                {
                    // Communicate ending of generation to GameStateManager
                    SetGameState(GameState.IN_GAME);
                    StartGame();
                    //UIManager.Instance.EnableLoadingScreen(false);
                    break;
                }
#endif
                GenManager.Instance.StartLevel();
                break;
            default:
                break;
        }
    }

    // Swaps the level to the given level
    public void HandleLevelSwap(int newLevelIndex)
    {
        // TODO: Save any necessary information from previous level
        SetIsGameActive(false);

        sceneLoader.LoadSceneWithId(newLevelIndex);
    }

    // Sets the time scale of the game to the given float
    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    // Quits the game
    public void QuitGame()
    {
        sceneLoader.Quit();
    }

    // Sets the gameState to the given gameState. This function should be managed carefully
    public void SetGameState(GameState _gameState, bool savePrev = false)
    {
#if UNITY_EDITOR
        Debug.Log($"Changing gamestate. Previous: [{prevGameState.ToString()}], New: [{_gameState.ToString()}]");
#endif

        // If the previous game state should be preserved, update it
        if (savePrev)
            prevGameState = this.gameState;
        // If the previous game state should not be preserved, update it to the current one
        else
            prevGameState = _gameState;

        gameState = _gameState;
        EvaluateGameStateEffects();
    }

    // Revers the gameState to the previous gameState. If there is no previous game state or the previous gameState is invalid, don't do anything
    public void RevertToPreviousGameState()
    {
        if (prevGameState != GameState.INVALID)
            gameState = prevGameState;

        EvaluateGameStateEffects();
    }

    // Acts on the effects of changing the gamestate
    private void EvaluateGameStateEffects()
    {
        switch (gameState)
        {
            // Pause the game for states where the game should be paused
            case GameState.LOADING_LEVEL:
            case GameState.PAUSED:
                SetTimeScale(0.0f);
                break;
            case GameState.MENU:
                SetTimeScale(1.0f);
                UIManager.Instance.HideUI();
                break;
            case GameState.IN_GAME:
                SetTimeScale(1.0f);
                UIManager.Instance.InitHUD();
                break;
            // For every other situation, do nothing but unpause
            default:
                SetTimeScale(1.0f);
                break;
        }
    }

    // Returns whether pausing is allowed currently based on the current gameState
    public bool CanPause()
    {
        switch(gameState)
        {
            // Only allow pausing in situations where the game is "active"
            case GameState.PAUSED:
            case GameState.IN_GAME:
                return true;
            // For every other situation, disable pausing
            default:
                return false;
        }
    }

    // Getters and setters
    public void SetIsGameActive(bool state) { IsGameActive = state; }
    public void SetIsEasyMode(bool _isEasy) { isEasyMode = _isEasy; }
    public bool GetIsEasyMode() { return isEasyMode; }
    public bool GetIsReady() { return isReady; }

    public int GetTrialNum() { return trialNum; }
    public void IncTrialNum() { trialNum++; }
}
