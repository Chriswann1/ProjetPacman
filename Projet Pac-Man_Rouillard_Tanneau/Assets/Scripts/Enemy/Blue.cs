using UnityEngine;

public class Blue : Enemy
{
    public override void Update()
    {
        target = GameObject.FindWithTag("Player").GetComponent<Transform>().position;
        base.Update();
    }
}
