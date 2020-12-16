using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{

    public static GameplayManager Instance;
    public int life;
    public GameObject player;

    public GameObject[] enemies;
    
    //Panel GameOver and Winning
    public GameObject panelWinning;
    public GameObject panelGameOver;
    
    public Text finalScoreTxt;
    public Text finalScoreWinTxt
        ;
    public Text gameTimeTxt;
    public Text gameTimeWinTxt;
    
    public Text pacGumTxt;
    public Text pacGumWinTxt;
    
    private static int gumDestroyed;
    private static int gumDestroyedWin;

    public int destroyedPacGum;

    //LifePrinting
    public GameObject life1, life2, life3;
    
    //Score and save score

    [SerializeField] private int lastScore = 0;
    public Text lastScoreTxt;
    public int score;
    public Text scoreTxt;
    
    //TimePrinting and Management 
    public Text timerText;
    private float startTime;
    private bool completedParty = false;
    
    //Sound

    public AudioClip defeatSound;
    public AudioSource audioSource;
    public AudioClip victorySound;

    // Start is called before the first frame update
    void Start()
    {
        SaveScore();
        life1.gameObject.SetActive(true);
        life2.gameObject.SetActive(true);
        life3.gameObject.SetActive(true);
        
        Instantiate(player, transform.position, transform.rotation);

        //Time printing
        startTime = Time.time;
        
    }
    
    void Awake()
    {
        if (Instance == null) {
            Instance = this;
        }else if(Instance != null){
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        //Time Management
        if (completedParty)
        {
            return;
        }
            
        float t = Time.time - startTime;

        string minutes = ((int) t / 60).ToString();
        string seconds = (t % 60).ToString("f1");

        timerText.text = "Temps : " + minutes + ":" + seconds;
        
        //We well prevent that the score be less than 0
        if (score <= 0)
        {
            score = 0;
        }
        if (player != null) { 
            scoreTxt.text = "Score : " + score;
            lastScoreTxt.text = "Dernier score : " + lastScore;
        }
        
        //We well prevent that the life be upper than 3
        if (life >= 3)
        {
            life = 3;
        }

        //We well prevent that the life be less than 0
        if (life <= 0)
        {
            life = 0;
        }
        
        switch (life)
        {
            case 4 :
                life1.gameObject.SetActive(true);
                life2.gameObject.SetActive(true);
                life3.gameObject.SetActive(true);
                break;
            case 3:
                life1.gameObject.SetActive(true);
                life2.gameObject.SetActive(true);
                life3.gameObject.SetActive(false);
                break;
            case 2:
                life1.gameObject.SetActive(true);
                life2.gameObject.SetActive(false);
                life3.gameObject.SetActive(false);
                break;
            case 1:
                life1.gameObject.SetActive(false);
                life2.gameObject.SetActive(false);
                life3.gameObject.SetActive(false);
                break;
        }
    }

    public void Mute()
    {
        AudioListener.pause = !AudioListener.pause;
    }

    public void SaveScore()
    {
        //This function will recover the score Set in Player's script for the PlayerPrefs
        if (PlayerPrefs.HasKey("Score"))
        {
            lastScore = PlayerPrefs.GetInt("Score");
            Debug.Log("Score chargé");
        }
    }

    public void ShowWin()
    {
        panelWinning.SetActive(true);
        //A winning sound will be played when the player will win the party
        audioSource.PlayOneShot(victorySound);
        
        gumDestroyedWin = destroyedPacGum;
        pacGumWinTxt.text = " PacGums : " + gumDestroyedWin;

        finalScoreWinTxt.text = "Score Final : " + score;
        completedParty = true;
        //gameTimeTxt.text = "Temps de jeu : " + timerText.text;

        float t = Time.time - startTime;

        string minutes = ((int) t / 60).ToString();
        string seconds = (t % 60).ToString("f1");
        gameTimeWinTxt.text = "Temps de jeu :  " + minutes + ":" + seconds;
    }
    
    public void ShowGameOver()
    {
        panelGameOver.SetActive(true);

        //A game over sound will be played when the player will be destroy
        audioSource.PlayOneShot(defeatSound);
        
        gumDestroyed = destroyedPacGum;
        pacGumTxt.text = " PacGums : " + gumDestroyed;

        finalScoreTxt.text = "Score Final : " + score;
        completedParty = true;
        
        float t = Time.time - startTime;

        string minutes = ((int) t / 60).ToString();
        string seconds = (t % 60).ToString("f1");
        gameTimeTxt.text = "Temps de jeu :  " + minutes + ":" + seconds;
    }
    public void onClick_Retry()
    {
        //SceneManager.UnloadSceneAsync(1);
        SceneManager.LoadScene("Level1");
    }
    public void onClick_Menu()
    {
        //SceneManager.UnloadSceneAsync(1);
        SceneManager.LoadScene("MainMenu");
    }
    public void exitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
        Debug.Log("Game is exiting");
    }
}
