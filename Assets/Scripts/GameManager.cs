using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance{get; private set;}

    public int maxBurnedTrees = 10;
    public int burnedTrees = 0;


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
            LoseGame();
        }

    }

    private void LoseGame()
    {
        Debug.Log("GAME OVER: too many trees burned!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}
