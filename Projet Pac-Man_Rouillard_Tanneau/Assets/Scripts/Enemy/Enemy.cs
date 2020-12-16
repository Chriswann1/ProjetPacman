using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

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
    private Vector3 nextwaypoint;
    
    [SerializeField] private float reflex;
    [SerializeField] private float speed;
    [SerializeField] private int PointToStart;
    
    public int id;
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
        
        if (GameplayManager.Instance.fear == true)
        {
            ActualBehaviour = BehaviourAI.Fleeing;
            this.GetComponent<SpriteRenderer>().color = Color.blue;
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

                        waypoints = _pathfinder.Pathfind(transform.position, target);
                        Debug.Log("Target is "+target);
                    }
                    yield return new WaitForSeconds(reflex);
                    break;
                case BehaviourAI.Fleeing:
                    target = _pathfinder.maptilemap.layoutGrid.GetCellCenterWorld(Pathfinder.TileNode.Keys.ToArray()[Random.Range(0, Pathfinder.TileNode.Keys.Count-1)]);
                    waypoints = _pathfinder.Pathfind(transform.position, target);
                    Debug.Log("Target is "+target);
                    
                    if (GameplayManager.Instance.fear == false)
                    {
                        ActualBehaviour = BehaviourAI.Attacking;
                        this.GetComponent<SpriteRenderer>().color = Color.white;
                    }

                    yield return new WaitForSeconds(reflex+1);
                    break;
                case BehaviourAI.Idle:
                    Debug.Log("In Idle");
                    if (GameplayManager.Instance.score > PointToStart)
                    {
                        ActualBehaviour = BehaviourAI.Attacking;
                    }
                    yield return new WaitForSeconds(reflex);
                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!GameplayManager.Instance.fear)
            {
                Destroy(other.gameObject);
                GameplayManager.Instance.ShowGameOver();
                
            }
            else
            {
                GameplayManager.Instance.score += 50;
                GameplayManager.Instance.RespawnEnemy(id);
                Destroy(this.gameObject);

            }
        }
    }
}
