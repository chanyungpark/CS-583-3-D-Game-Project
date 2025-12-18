using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Score Values")]
    public int hikersSaved = 0;
    public int firesExtinguished = 0;

    [Header("UI References")]
    public TextMeshProUGUI hikersText;
    public TextMeshProUGUI firesText;
    public TextMeshProUGUI timeText;

    private float elapsedTime;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Update()
    {
        UpdateTime();
        UpdateUI();
    }

    void UpdateTime()
    {
        elapsedTime += Time.deltaTime;
    }

    void UpdateUI()
    {
        hikersText.text = $"HIKERS SAVED: {hikersSaved}";
        firesText.text = $"FIRES EXTINGUISHED: {firesExtinguished}";

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timeText.text = $"{minutes:00}:{seconds:00}";
    }

    // Public methods to call from gameplay scripts
    public void AddHikerSaved()
    {
        hikersSaved++;
    }

    public void AddFireExtinguished()
    {
        firesExtinguished++;
    }
}