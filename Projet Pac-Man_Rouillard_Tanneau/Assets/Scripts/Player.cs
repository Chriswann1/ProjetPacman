using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{

    private GameObject ball;
    //public static int destroyedPacGum;

    /// PlayerMovement
    public bool Isplay;
    public float speed;
    private Vector3Int direction = Vector3Int.zero;
    private Vector3Int currentdirection = Vector3Int.zero;
    public Vector3Int target = Vector3Int.zero;
    private Vector3Int actualtilepos = Vector3Int.zero;
    public Vector3 targetworld = Vector3.zero;
    private bool inmovement = false;
    private float starttime;
    private float lerppos = 0;
    [SerializeField] private Vector3Int[] portals;
    /// /PlayerMovement
    
    /// tilemap
    private Tilemap tilemap;
    /// /tilemap
    
    /// sound
    [SerializeField] private AudioClip powersound;
    /// /sound



    private void Awake()//used to get the tilemap
    {
        tilemap = GameObject.FindWithTag("GameplayManager").GetComponent<Pathfinder>().maptilemap;
    }

    void Start()//Used to reset Pacgun in gameplaymanager
    {

        Reset();

    }

    void Update() //Used for movement
    {
        actualtilepos = target;
        if (Isplay)
        {
            CheckInput();
        }

        lerppos = (Time.time - starttime) * speed;
        transform.position = Vector3.Lerp(tilemap.layoutGrid.GetCellCenterWorld(actualtilepos), tilemap.layoutGrid.GetCellCenterWorld(target), lerppos);
        if (lerppos >= 1f)
        {
            inmovement = false;
            this.GetComponent<Animator>().SetBool("movement", false);
        }
    }

    void Reset()
    {
        GameplayManager.Instance.destroyedPacGum = 0;
    }

    void CheckInput() //Used to check if we can move and get input from Vertical and Horizontal Axis (ZQSD + Arrows)
    {
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");
        direction = new Vector3Int(Mathf.RoundToInt(horizontal), Mathf.RoundToInt(vertical), 0);

        if (direction.x != 0 && direction.y != 0)
        { 
            direction.y = 0;
        }

        if (!tilemap.GetTile(actualtilepos + direction) && direction != Vector3Int.zero)
        { 
            currentdirection = direction;
        }

        if (!inmovement && !tilemap.GetTile(actualtilepos + currentdirection))
        {
            starttime = Time.time;
            if (actualtilepos + currentdirection == portals[0])
            {
                target = portals[1] + Vector3Int.left;
                transform.position = tilemap.layoutGrid.GetCellCenterWorld(portals[1]);
                direction = Vector3Int.left;
                currentdirection = Vector3Int.left;
            }else if (actualtilepos + currentdirection == portals[1])
            {
                target = portals[0] + Vector3Int.right;
                transform.position = tilemap.layoutGrid.GetCellCenterWorld(portals[0]);
                direction = Vector3Int.right;
                currentdirection = Vector3Int.right;
            }
            else
            { 
                target = actualtilepos + currentdirection; 
            }
            targetworld = tilemap.layoutGrid.GetCellCenterWorld(target); 
            inmovement = true;
            this.GetComponent<Animator>().SetBool("movement", true);
            Orientation();
            }
    }

    void Orientation()//used to set the orientation of the sprite
    {
        if (currentdirection == Vector3Int.left)
        {
            transform.localScale = new Vector3(-1, -1, 1);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (currentdirection == Vector3Int.right)
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (currentdirection == Vector3Int.up)
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.localRotation = Quaternion.Euler(0, 0, 90);
        }
        else if (currentdirection == Vector3Int.down)
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.localRotation = Quaternion.Euler(0, 0, 270);
        }
    }

    void OnTriggerEnter2D(Collider2D other)//Used to detect Ball and powerball, enemy process to the "death" detection if collide with player
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            Destroy(other.gameObject);
            GameplayManager.Instance.score += 10;
            GameplayManager.Instance.destroyedPacGum += 1;
            GameplayManager.Instance.remainingpoints--;
            
        }
        else if (other.gameObject.CompareTag("PowerBall"))
        { 
            Destroy(other.gameObject); 
            GameplayManager.Instance.score += 50; 
            this.GetComponent<AudioSource>().clip = powersound; 
            this.GetComponent<AudioSource>().Play(); 
            StartCoroutine(GameplayManager.Instance.FearPower());
        }
    }
    
}
