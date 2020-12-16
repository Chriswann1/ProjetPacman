using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;


public class Pathfinder : MonoBehaviour
{
    public static Dictionary<Vector3Int, Node> TileNode;
    public Tilemap maptilemap;
    [SerializeField] private Vector3Int startingtile;

    [SerializeField] private int overflowlimit;
    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject energizer;
    [SerializeField] private int energizerchance;
    [SerializeField] private Vector3Int enemyspawndoor;
    private bool inbase = true;


    private void Awake()
    {
        
        Queue<Vector3Int> NodeQueue = new Queue<Vector3Int>();
        NodeQueue.Enqueue(startingtile);
        TileNode = new Dictionary<Vector3Int, Node>();
        
        while (NodeQueue.Count > 0)
        {
            List<Vector3Int> children = new List<Vector3Int>();
            Vector3Int actualnode = NodeQueue.Dequeue();
            if (actualnode == enemyspawndoor)
            {
                inbase = false;
            }
            if (!TileNode.ContainsKey(actualnode))
            {
                Debug.Log(actualnode);
                if (maptilemap.GetTile(actualnode+new Vector3Int(1,0,0)) == null)
                {
                    NodeQueue.Enqueue(actualnode+new Vector3Int(1,0,0));
                    children.Add(actualnode+new Vector3Int(1,0,0));
                }
                if (maptilemap.GetTile(actualnode+new Vector3Int(-1,0,0)) == null)
                {
                    NodeQueue.Enqueue(actualnode+new Vector3Int(-1,0,0));
                    children.Add(actualnode+new Vector3Int(-1,0,0));
                }
                if (maptilemap.GetTile(actualnode+new Vector3Int(0,1,0)) == null)
                {
                    NodeQueue.Enqueue(actualnode+new Vector3Int(0,1,0));
                    children.Add(actualnode+new Vector3Int(0,1,0));
                }
                if (maptilemap.GetTile(actualnode+new Vector3Int(0,-1,0)) == null)
                {
                    NodeQueue.Enqueue(actualnode+new Vector3Int(0,-1,0));
                    children.Add(actualnode+new Vector3Int(0,-1,0));
                }
                TileNode.Add(actualnode, new Node(children, actualnode, null));
                if (!inbase)
                {
                    if (Random.Range(0,101) <= energizerchance)
                    {
                        Instantiate(energizer, maptilemap.layoutGrid.GetCellCenterWorld(actualnode), transform.rotation);
                    }
                    else
                    {
                        Instantiate(ball, maptilemap.layoutGrid.GetCellCenterWorld(actualnode), transform.rotation);
                    }
                }

                foreach (Vector3Int child in children)
                {
                   
                    Debug.DrawLine(maptilemap.layoutGrid.GetCellCenterWorld(actualnode), maptilemap.layoutGrid.GetCellCenterWorld(child), Color.white, 10);
                    Debug.Log(child+" is Child of "+actualnode);

                }
            }
            else
            {
                Debug.Log("Already visited");
            }
        }
        Debug.Log("PathConstructor end");
        
        //StartCoroutine(constructor());
    }

    public Stack<Vector3> Pathfind (Vector3 From_world, Vector3 To_world)
    {
        List<Node> usedNode = new List<Node>();
        if (TileNode.ContainsValue(TileNode[maptilemap.layoutGrid.WorldToCell(From_world)]) && TileNode.ContainsValue(TileNode[maptilemap.layoutGrid.WorldToCell(To_world)]))
        {
            Node From = TileNode[maptilemap.layoutGrid.WorldToCell(From_world)];
            Node To = TileNode[maptilemap.layoutGrid.WorldToCell(To_world)];
        
            Queue<Node> NodeQueue = new Queue<Node>();
            NodeQueue.Enqueue(From);
            int overflowlevel = 0;
            while (NodeQueue.Count > 0)
            {
                overflowlevel++;
                if (overflowlevel > overflowlimit)
                {
                    //Debug.Log("Overflow Limit");
                    return new Stack<Vector3>();
                }
                Node thisnode = NodeQueue.Dequeue();
                //Debug.Log("Actual Node is "+thisnode.position);
                if (thisnode.position != To.position)
                {
                
                    foreach (Vector3Int child in thisnode.children)
                    {
                        if (TileNode[child].parent == null)
                        {
                            TileNode[child].parent = thisnode;
                            usedNode.Add(TileNode[child]);
                            NodeQueue.Enqueue(TileNode[child]);
                            //Debug.Log("Adding Child to queue");
                        }
                    }
                }
                else
                {
                    //Debug.Log("Going to Reverse Path");
                    return ReversePathfind(thisnode, From, usedNode);
                }

            }
            //Debug.Log("While end");
            return new Stack<Vector3>();
        }
        else
        {
            return new Stack<Vector3>();
        }

    }

    public Stack<Vector3> ReversePathfind(Node Target, Node Root, List<Node> usedNode)
    {
        if (Target == Root)
        {
            return new Stack<Vector3>();
        }
        Stack<Vector3> path = new Stack<Vector3>();
        path.Push(maptilemap.layoutGrid.GetCellCenterWorld(Target.position));
        Debug.DrawLine(maptilemap.layoutGrid.GetCellCenterWorld(Target.position), maptilemap.layoutGrid.GetCellCenterWorld(Target.parent.position), Color.red, 1);
        Node actualnode = Target.parent;



        while (actualnode != Root)
        {
            //Debug.Log("InReversePath with "+ actualnode.position);
            path.Push(maptilemap.layoutGrid.GetCellCenterWorld(actualnode.position));
            Debug.DrawLine(maptilemap.layoutGrid.GetCellCenterWorld(actualnode.position), maptilemap.layoutGrid.GetCellCenterWorld(actualnode.parent.position), Color.red, 1);
            actualnode = actualnode.parent;
            

        }
        
        //path.Push(maptilemap.layoutGrid.GetCellCenterWorld(actualnode.position));
        actualnode.parent = null;
        foreach (Node used in usedNode)
        {
            used.parent = null;
        }
        return path;
    }

    
}
