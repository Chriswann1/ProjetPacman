using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    
    //PlayerMovement
    public float speed;

    private Vector2 direction = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        CheckInput();
        move();
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
        }else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction = Vector2.right;
        }else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction = Vector2.up;
        }else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction = Vector2.down;
        }
       
    }

    void move()
    {
        transform.localPosition += (Vector3) (direction * speed) * Time.deltaTime;
    }
}
