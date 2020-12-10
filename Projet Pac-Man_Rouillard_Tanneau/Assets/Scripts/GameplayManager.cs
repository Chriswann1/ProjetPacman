using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{

    public static GameplayManager Instance;
    public int life;
    public GameObject player;
    public GameObject playerIngame;
    
    //LifePrinting
    public GameObject life1, life2, life3;
    
    //Score
    public float score;

    public Text scoreTxt;
    // Start is called before the first frame update
    void Start()
    {
        life1.gameObject.SetActive(true);
        life2.gameObject.SetActive(true);
        life3.gameObject.SetActive(true);
        
        Instantiate(player, transform.position, transform.rotation);
        //player.transform.localScale = new Vector3(2,2,0);
    }

    public void Respawn()
    {
        Instantiate(player, transform.position, transform.rotation);
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
        //We well prevent that the score be less than 0
        if (score <= 0)
        {
            score = 0;
        }
        if (player != null) { 
            scoreTxt.text = "Score : " + score;
        }
        
        if (life >= 3)
        {
            life = 3;
        }

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
}
