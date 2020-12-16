using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum BehaviourAI
    {
        Idle,
        Attacking,
        Fleeing
        
    }
    private BehaviourAI ActualBehaviour = BehaviourAI.Idle;

    private Stack<Vector3> waypoints;
    private Vector3 nextwaypoint;
    [SerializeField] private float reflex;
    [SerializeField] private float speed;
    [SerializeField] private float starttime;
    protected Vector3 target;
    

    [SerializeField]
    public Pathfinder _pathfinder;

    public virtual void Start()
    {
        waypoints = new Stack<Vector3>();
        _pathfinder = GameObject.FindWithTag("GameplayManager").GetComponent<Pathfinder>();
        StartCoroutine(FindPath());
    }
    
    public virtual void Update()
    {
        if (waypoints.Count > 0)
        {
            if (transform.position != nextwaypoint && nextwaypoint != Vector3.zero)
            {
                this.GetComponent<Animator>().SetFloat("Yvector", nextwaypoint.y  - transform.position.y);
                this.GetComponent<Animator>().SetFloat("Xvector", nextwaypoint.x  - transform.position.x);
                transform.position = Vector3.MoveTowards(transform.position, nextwaypoint, speed * Time.deltaTime);
            }
            else
            {
                nextwaypoint = waypoints.Pop();
            }
        }
    }

    public virtual IEnumerator FindPath()
    {
        while (true)
        {
            switch (ActualBehaviour)
            {
                case BehaviourAI.Attacking:
                    Debug.Log("In Attack");
                    if (target != Vector3.zero)
                    {
                        waypoints.Clear();
                        waypoints = _pathfinder.Pathfind(transform.position, target);

                    }
                    yield return new WaitForSeconds(reflex);
                    break;
                
                case BehaviourAI.Fleeing:
                    
                    yield return new WaitForSeconds(reflex);
                    break;
                
                case BehaviourAI.Idle:
                    Debug.Log("In Idle");
                    yield return new WaitForSeconds(starttime);
                    ActualBehaviour = BehaviourAI.Attacking;
                    break;

                
            }
        }
    }
}
