using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] private GameObject hudCanvas;

    public static GameManager Instance{get; private set;}

    public int maxBurnedTrees = 10;
    public int burnedTrees = 0;

    private bool isPaused = false;
    private bool gameOver = false;

    public bool IsPaused => isPaused;
    public bool IsGameOver => gameOver;

    [Header("Hiker Stats")]
    public int savedHikers = 0;
    public int totalHikersInMap = 0; 
    public int totalHikersLost = 0;  
    public int hickersSpawned;   

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ShowHUD(bool show)
    {
        if (hudCanvas != null)
            hudCanvas.SetActive(show);
    }

    public void SetPaused(bool paused)
    {
        if (gameOver) return;

        isPaused = paused;
        Time.timeScale = paused ? 0f : 1f;
        ShowHUD(!paused);
    }

    public void TreeBurned()
    {
        burnedTrees++;
        Debug.Log("Tree burned! Total: " + burnedTrees);

        if(burnedTrees >= maxBurnedTrees)
        {
            LoseGame("The forest was overwhelmed by fire...");
        }
    }

    public void HikerLoss()
    {
        totalHikersLost++;
 
        if (totalHikersLost >= 2) 
        {
            LoseGame("Too many Hikers lost to the fire.");
        }
    }

    public void PlayerDied()
    {
        LoseGame("You died.");
    }

    private void LoseGame(string reason)
    {
        if (gameOver) return;
        gameOver = true;

        Time.timeScale = 0f;
        ShowHUD(false);

        Debug.Log("GAME OVER: " + reason);

        if (GameOverUI.Instance != null)
        {
            GameOverUI.Instance.ShowGameOver(reason);
        }
    }

    public void HikerSaved() 
    {
        savedHikers++;
        Debug.Log($"Hiker saved! Total: {savedHikers}/{totalHikersInMap}");

        if (totalHikersInMap > 0 && savedHikers >= (totalHikersInMap - 1))
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        if (gameOver) return;
    gameOver = true;

    Time.timeScale = 0f;
    ShowHUD(false);

    Debug.Log("YOU WIN! Most hikers rescued.");
    
    if (GameOverUI.Instance != null)
    {
        GameOverUI.Instance.ShowGameOver("YOU HAVE SAVED ENOUGH HIKERS! THE FORREST IS SAFE.");
    }
    }
}
