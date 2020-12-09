using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Player : MonoBehaviour
{

    private GameObject ball;
    
    //PlayerMovement
    public float speed;
    private bool onMovement;
    private Vector2 direction = Vector2.zero;
    
    //LifePrinting
    //public GameObject life1, life2, life3;
    
    //scoreManagement
    public float score;
    
    //HealthManagement
    public float life = 3;
    
    // Start is called before the first frame update
    void Start()
    {
        /*life1.gameObject.SetActive(true);
        life2.gameObject.SetActive(true);
        life3.gameObject.SetActive(true);*/
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        move();
        Orientation();

        //Life system is 
        /*if (life >= 3)
        {
            life = 3;
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
        }*/
    }

    private void FixedUpdate()
    {
        
        /*float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
 
        movement = new Vector3(moveHorizontal, moveVertical, 0f );
 
        movement = movement * speed * Time.deltaTime;
 
        transform.position += movement;*/
    }

    void CheckInput()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction = Vector2.left;
            onMovement = true;
        }else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction = Vector2.right;
            onMovement = true;
        }else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction = Vector2.up;
            onMovement = true;
        }else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction = Vector2.down;
            onMovement = true;
        }
       
    }

    void move()
    {
        if (onMovement == true)
        {
            transform.localPosition += (Vector3) (direction * speed) * Time.deltaTime;
        }
    }

    void Orientation()
    {
        if (direction == Vector2.left)
        {
            transform.localScale = new Vector3(-1,-1,1);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (direction == Vector2.right)
        {
            transform.localScale = new Vector3(1,1,1);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (direction == Vector2.up)
        {
            transform.localScale = new Vector3(1,1,1);
            transform.localRotation = Quaternion.Euler(0, 0, 90);
        }
        else if (direction == Vector2.down)
        {
            transform.localScale = new Vector3(1,1,1);
            transform.localRotation = Quaternion.Euler(0, 0, 270);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag ("Ball"))
        {
            Destroy(other.gameObject);
            GameplayManager.Instance.score += 10;
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
            GameplayManager.Instance.life -= 1;
        }
        else
        {
            onMovement = false;
        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Map"))
        {
            
        }
    }*/
}
