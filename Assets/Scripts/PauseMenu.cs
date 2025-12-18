using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    public GameObject pauseMenuUI;
    public Slider masterVolumeSlider;
    public Toggle sfxMuteToggle;
    public Toggle musicMuteToggle;

    [Header("Audio")]
    public AudioMixer audioMixer;

    private bool isPaused = false;
    private float lastSFXVolume = 1f;

    void Start()
    {
        pauseMenuUI.SetActive(false);

        // Load saved settings
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        sfxMuteToggle.isOn = PlayerPrefs.GetInt("SFXMuted", 0) == 1;
        musicMuteToggle.isOn = PlayerPrefs.GetInt("MusicMuted", 0) == 1;

        ApplyAudioSettings();
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        GameManager.Instance.SetPaused(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        GameManager.Instance.SetPaused(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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

    // ================= AUDIO =================

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void ToggleSFXMute(bool isMuted)
    {
        float volume = isMuted ? -80f : 0f;

        audioMixer.SetFloat("SFXVolume", volume);
        PlayerPrefs.SetInt("SFXMuted", isMuted ? 1 : 0);

        Debug.Log("SFX Muted: " + isMuted);

        if (isMuted)
        {
            audioMixer.SetFloat("SFXVolume", -80f);
        }
        else
        {
            audioMixer.SetFloat("SFXVolume", Mathf.Log10(lastSFXVolume) * 20);
        }
    }

    public void ToggleMusicMute(bool isMuted)
    {
        audioMixer.SetFloat("MusicVolume", isMuted ? -80f : 0f);
        PlayerPrefs.SetInt("MusicMuted", isMuted ? 1 : 0);
    }

    void ApplyAudioSettings()
    {
        SetMasterVolume(masterVolumeSlider.value);
        ToggleSFXMute(sfxMuteToggle.isOn);
        ToggleMusicMute(musicMuteToggle.isOn);
    }
}
