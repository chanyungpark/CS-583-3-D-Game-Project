using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Call this from the Play button
    public void PlayGame()
    {
        // Replace "Game" with the name of your gameplay scene
        SceneManager.LoadScene("Gameplay");
    }

    // Call this from the Quit button
    public void QuitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();

        // Note: Application.Quit() only works in a build,
        // not in the editor.
    }
}