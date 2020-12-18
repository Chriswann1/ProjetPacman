using UnityEngine;

public class Red : Enemy
{
    
    public override void Update() //Overriding the Update of Enemy class to change the target function
    {
        if (GameObject.FindWithTag("Player") != null)
        {
            Debug.Log("Player targeted");
            target = GameObject.FindWithTag("Player").GetComponent<Transform>().position;
            base.Update();
        }


    }

}
