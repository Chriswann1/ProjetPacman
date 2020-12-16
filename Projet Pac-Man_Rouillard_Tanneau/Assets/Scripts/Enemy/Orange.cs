using UnityEngine;

public class Orange : Enemy
{
    public override void Update()
    {
        target = GameObject.FindWithTag("Player").GetComponent<Transform>().position;
        base.Update();
    }
}
