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
    public BehaviourAI ActualBehaviour = BehaviourAI.Idle;
    
    
    private Stack<Vector3> waypoints;
    
    public int seed;
    public int reflex;
    [SerializeField]private int speed;
    [SerializeField] private float starttime;
    
    
    private Vector3 position;
    private GameObject Player;
    [SerializeField]
    private Pathfinder _pathfinder;

    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        _pathfinder = GameObject.FindWithTag("GameplayManager").GetComponent<Pathfinder>();
        StartCoroutine(FindPath());
    }

    void Update()
    {
        
    }

    IEnumerator FindPath()
    {
        while (true)
        {
            switch (ActualBehaviour)
            {
                case BehaviourAI.Attacking:
                    waypoints = _pathfinder.Pathfind(position, Player.transform.position);
                    yield return new WaitForSeconds(reflex);
                    break;
                
                case BehaviourAI.Fleeing:
                    
                    yield return new WaitForSeconds(reflex);
                    break;
                
                case BehaviourAI.Idle:
                    yield return new WaitForSeconds(starttime);
                    ActualBehaviour = BehaviourAI.Attacking;
                    break;

                
            }
        }
    }
    
    
}
