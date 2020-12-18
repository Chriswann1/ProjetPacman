
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    /// audio
    public AudioSource audioSource;
    public AudioClip introSound;
    /// /audio

    public void StartGame() //used by a button to load scene Level1
    {
        SceneManager.LoadScene(1);
    }
    public void Mute() //used by a button to mute music
    {
        AudioListener.pause = !AudioListener.pause;
    }

    public void exitGame() //used by a button to quit the game
    {
        Application.Quit();
        Debug.Log("Game is closed");
    }
}
