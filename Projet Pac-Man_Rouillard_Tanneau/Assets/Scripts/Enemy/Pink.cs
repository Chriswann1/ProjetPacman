using UnityEngine;

public class Pink : Enemy
{
    public override void Update()
    {
        if (GameObject.FindWithTag("Player") != null)
        {
            Debug.Log("Player targeted");
            target = GameObject.FindWithTag("Player").GetComponent<Player>().targetworld;
            base.Update();
        }

    }
}
