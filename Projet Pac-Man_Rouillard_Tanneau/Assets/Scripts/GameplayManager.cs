using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    /// GameplayManagerInstance
    public static GameplayManager Instance;
    /// /GameplayManagerInstance

    /// Player Settings
    public int life;
    public GameObject playerpref;
    public Vector3Int spawn;
    /// /Player Settings

    /// EnnemiesSettings
    [SerializeField] private GameObject[] enemiesPref;
    [SerializeField] private GameObject enemy;
    [SerializeField] private Vector3Int[] spawnpos;
    /// /EnemiesSettings

    /// Panel GameOver and Winning
    public GameObject panelWinning;
    public GameObject panelGameOver;
    /// Panel GameOver and Winning
    
    /// All text variables
    public Text finalScoreTxt;
    public Text finalScoreWinTxt;
    public Text gameTimeTxt;
    public Text gameTimeWinTxt;
    public Text pacGumTxt;
    public Text pacGumWinTxt;
    /// /All text variables

    /// Gun management
    private static int gumDestroyed;
    private static int gumDestroyedWin;
    public int destroyedPacGum;
    /// /Gun management
    
    
    /// Score and save score
    [SerializeField] private int lastScore = 0;
    public Text lastScoreTxt;
    public int score;
    public Text scoreTxt;
    public int remainingpoints;
    /// /Score and save score
    
    /// TimePrinting and Management 
    public Text timerText;
    private float startTime;
    private bool completedParty = false;
    /// /TimePrinting and Management
    
    
    /// Sound
    public AudioClip defeatSound;
    public AudioSource audioSource;
    public AudioClip victorySound;
    /// /Sound
    
    /// fear system
    public bool fear = false;
    [SerializeField] private float feartime;
    /// /fear system
    
    /// Pathfinder variable
    private Pathfinder _pathfinder;
    /// /Pathfinder variable
    
    // Start is called before the first frame update
    void Start() //Use start to set player settings, points, Score, time and ennemies
    {
        remainingpoints = _pathfinder.ballsnumber;
        Instantiate(playerpref, _pathfinder.maptilemap.layoutGrid.GetCellCenterWorld(spawn), transform.rotation).GetComponent<Player>().target = spawn;
        for (int i = 0; i < enemiesPref.Length; i++)
        {
            enemy = Instantiate(enemiesPref[i], _pathfinder.maptilemap.layoutGrid.GetCellCenterWorld(spawnpos[i]), transform.rotation);
            enemy.GetComponent<Enemy>().id = i;
        }

        //Time printing
        startTime = Time.time;
        SaveScore();
    }

    void Awake() // use awake to initialize gameplay manager instance and get pathfinder
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }

        _pathfinder = this.GetComponent<Pathfinder>();
    }

    // Update is called once per frame
    void Update() //use it to manage time, score, win and lose + old life system
    {

        
        //Time Management
        if (completedParty)
        {
            return;
        }
        float t = Time.time - startTime;
        
        if (remainingpoints <= 0)
        {
            ShowWin();
            Mathf.Clamp(remainingpoints, 0, Mathf.Infinity);
        }
        string minutes = ((int) t / 60).ToString();
        string seconds = (t % 60).ToString("f1");

        timerText.text = "Temps : " + minutes + ":" + seconds;

        //We well prevent that the score be less than 0
        if (score <= 0)
        {
            score = 0;
        }

        if (playerpref != null)
        {
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


        
    }
    
    public void Mute() //Used to mute music
    {
        AudioListener.pause = !AudioListener.pause;
    }

    public void SaveScore() //This function will recover the score Set in Player's script for the PlayerPrefs
    {

        if (PlayerPrefs.HasKey("Score"))
        {
            lastScore = PlayerPrefs.GetInt("Score");
            Debug.Log("Score chargé");
        }
    }

    public IEnumerator FearPower() //used to set a "timer" between fear and not in fear mode
    {
        fear = true;
        yield return new WaitForSeconds(feartime);
        fear = false;

        yield return null;
    }

    public void RespawnEnemy(int id) //Make respawn the enemy that identity his prefab 
    {
        GameObject spawned = Instantiate(enemiesPref[id],
            _pathfinder.maptilemap.layoutGrid.GetCellCenterWorld(spawnpos[id]),
            transform.rotation);
        spawned.GetComponent<Enemy>().id = id;

    }

    public void ShowWin() //activate the Win menu
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

        if (score > lastScore)
        {
            PlayerPrefs.SetInt("Score", score);
        }
    }

    public void ShowGameOver() //activate the Game Over menu
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
        if (score > lastScore)
        {
            PlayerPrefs.SetInt("Score", score);
        }
    }

    public void Setplayeractive()//used by the text/button at the start of the game to make the player able to move
    {
        if (GameObject.FindWithTag("Player") != null)
        {
            GameObject.FindWithTag("Player").GetComponent<Player>().Isplay = true;
        }
        
    }

    public void onClick_Retry()//used by a button to reload the scene
    {
        //SceneManager.UnloadSceneAsync(1);
        SceneManager.LoadScene("Level1");
    }

    public void onClick_Menu()//used by a button to load the main menu scene
    {
        //SceneManager.UnloadSceneAsync(1);
        SceneManager.LoadScene("MainMenu"); 
    }

    public void exitGame()//used by a button to quit the game
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }
}


