using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance{get; private set;}

    public int maxBurnedTrees = 10;
    public int burnedTrees = 0;

    private bool gameOver = false;


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

    public void TreeBurned()
    {
        burnedTrees++;
        Debug.Log("Tree burned! Total: " + burnedTrees);

        if(burnedTrees >= maxBurnedTrees)
        {
            LoseGame("The forest was overwhelmed by fire...");
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

        Debug.Log("GAME OVER: " + reason);

        if (GameOverUI.Instance != null)
        {
            GameOverUI.Instance.ShowGameOver(reason);
        }
    }
}
