using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{

    private GameObject ball;
    //public static int destroyedPacGum;

    //PlayerMovement
    public float speed;

    private Vector3Int direction = Vector3Int.zero;
    private Vector3Int currentdirection = Vector3Int.zero;
    private Vector3Int target = Vector3Int.zero;
    private Vector3Int actualtilepos = Vector3Int.zero;
    private Tilemap tilemap;
    public Vector3 targetworld = Vector3.zero;
    [SerializeField] private int scoretowin;
    private bool inmovement = false;
    private float starttime;
    private float lerppos = 0;



    //scoreManagement
    public float score;

    //HealthManagement
    public float life = 3;

    // Update is called once per frame

    private void Awake()
    {
        tilemap = GameObject.FindWithTag("GameplayManager").GetComponent<Pathfinder>().maptilemap;
    }

    // Start is called before the first frame update
    void Start()
    {
        Reset();

    }

    void Update()
    {

        actualtilepos = target;
        CheckInput();
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

    void CheckInput()
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
            target = actualtilepos + currentdirection; 
            targetworld = tilemap.layoutGrid.GetCellCenterWorld(target); 
            inmovement = true;
            this.GetComponent<Animator>().SetBool("movement", true);
            Orientation();
            }




    }

    void Orientation()
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            Destroy(other.gameObject);
            GameplayManager.Instance.score += 10;
            GameplayManager.Instance.destroyedPacGum += 1;
            if (GameplayManager.Instance.destroyedPacGum >= scoretowin)
            { 
                GameplayManager.Instance.ShowWin();
            }

                //onMovement = true;
            }
            else if (other.gameObject.CompareTag("PowerBall"))
            {
                Destroy(other.gameObject);
                GameplayManager.Instance.score += 50;
            }
            else if (other.gameObject.CompareTag("Enemy"))
            {
                Destroy(other.gameObject);
                Destroy(this.gameObject);
                GameplayManager.Instance.ShowGameOver();
                //GameplayManager.Instance.life -= 1;
            }





    }


    

}
