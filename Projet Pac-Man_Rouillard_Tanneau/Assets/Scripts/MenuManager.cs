
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip introSound;
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void Mute()
    {
        AudioListener.pause = !AudioListener.pause;
    }

    public void exitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
        Debug.Log("Game is closed");
    }
}
