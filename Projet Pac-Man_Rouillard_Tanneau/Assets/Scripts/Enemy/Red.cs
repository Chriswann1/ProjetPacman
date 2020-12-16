using UnityEngine;

public class Red : Enemy
{
    
    public override void Update()
    {
        if (GameObject.FindWithTag("Player") != null)
        {
            Debug.Log("Player targeted");
            target = GameObject.FindWithTag("Player").GetComponent<Transform>().position;
            base.Update();
        }


    }

}
