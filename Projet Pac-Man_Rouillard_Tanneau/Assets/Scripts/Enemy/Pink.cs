using UnityEngine;

public class Pink : Enemy
{
    public override void Update()
    {
        target = GameObject.FindWithTag("Player").GetComponent<Player>().targetworld;
        base.Update();
    }
}
