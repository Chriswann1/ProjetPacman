using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Player : MonoBehaviour
{

    private GameObject ball;
    //public static int destroyedPacGum;

    //PlayerMovement
    public float speed;
    private bool onMovement;
    private Vector2 direction = Vector2.zero;
    
    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        move();
        Orientation();
    }

    private void Reset()
    {
        GameplayManager.Instance.destroyedPacGum = 0;
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
            GameplayManager.Instance.destroyedPacGum += 1;
            if (GameplayManager.Instance.destroyedPacGum == 5)
            {
                GameplayManager.Instance.ShowWin();
                onMovement = false;
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
        else
        {
            onMovement = false;
        }
    }
    
}
