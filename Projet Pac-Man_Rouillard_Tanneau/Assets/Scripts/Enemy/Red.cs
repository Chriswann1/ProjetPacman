using UnityEngine;

public class Red : Enemy
{
    
    public override void Update()
    {
        target = GameObject.FindWithTag("Player").GetComponent<Transform>().position;
        base.Update();
    }

}
