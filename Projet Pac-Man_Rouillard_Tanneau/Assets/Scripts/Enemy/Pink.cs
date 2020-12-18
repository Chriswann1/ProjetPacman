using UnityEngine;

public class Pink : Enemy
{
    public override void Update() //Overriding the Update of Enemy class to change the target function
    {
        if (GameObject.FindWithTag("Player") != null)
        {
            Debug.Log("Player targeted");
            target = GameObject.FindWithTag("Player").GetComponent<Player>().targetworld; // the target is the target of the play (which is the next point where the player is heading ahead)
            base.Update();
        }

    }
}
