using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance;

    public GameObject root;
    public TMP_Text finalScoreText;
    public TMP_Text reasonText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        root.SetActive(false);
    }

    public void ShowGameOver(string reason)
    {
        root.SetActive(true);
        reasonText.text = reason;

        if (ScoreManager.Instance != null && finalScoreText != null)
        {
            int finalScore = ScoreManager.Instance.GetFinalScore();
            finalScoreText.text = $"SCORE: {finalScore}";
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}